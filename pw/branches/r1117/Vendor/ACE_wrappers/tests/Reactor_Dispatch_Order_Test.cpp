// $Id: Reactor_Dispatch_Order_Test.cpp 85615 2009-06-12 01:21:57Z shuston $

// ============================================================================
//
// = LIBRARY
//    tests
//
// = FILENAME
//    Reactor_Dispatch_Order_Test.cpp
//
// = DESCRIPTION
//    This is a simple test that checks the order of dispatching of
//    ACE Reactors.  Order should be: timeout, output, and then input.
//
// = AUTHOR
//    Irfan Pyarali <irfan@cs.wustl.edu>
//
// ============================================================================

#include "test_config.h"
#include "ace/OS_NS_string.h"
#include "ace/Reactor.h"
#include "ace/Select_Reactor.h"
#include "ace/WFMO_Reactor.h"
#include "ace/Dev_Poll_Reactor.h"
#include "ace/Pipe.h"
#include "ace/ACE.h"

ACE_RCSID(tests, Reactor_Dispatch_Order_Test, "$Id: Reactor_Dispatch_Order_Test.cpp 85615 2009-06-12 01:21:57Z shuston $")

static const char *message =
"Hello there! Hope you get this message";

class Handler : public ACE_Event_Handler
{
public:
  Handler (ACE_Reactor &reactor);

  ~Handler();

  int handle_timeout (const ACE_Time_Value &tv,
                      const void *arg);

  int handle_input (ACE_HANDLE fd);

  int handle_output (ACE_HANDLE fd);

  ACE_HANDLE get_handle (void) const;

  // We need to add MSG_OOB data transfer to this test to check the
  // order of when <handle_exception> gets called.  I tried with
  // Windows 2000 but only one byte of the message came across as
  // urgent data.  The rest of the data was treated as normal! There
  // was some explanation of Microsoft's TCP/IP deals with out-of-band
  // data in "Out-of-Band Data and Push Bit in TCP/IP" in the MSDN
  // library.  However, I did not comprehend that well enough.  If
  // someone can make this work, please check the order of
  // <handle_exception> getting called.
  // int handle_exception (ACE_HANDLE fd);

  ACE_Pipe pipe_;

  int dispatch_order_;
  bool ok_;           // Constructed and initialized ok
};

Handler::Handler (ACE_Reactor &reactor)
  : ACE_Event_Handler (&reactor),
    dispatch_order_ (1),
    ok_ (false)
{
  // Create the pipe.
  if (0 != this->pipe_.open ())
    ACE_ERROR ((LM_ERROR, ACE_TEXT ("%p\n"), ACE_TEXT ("pipe")));
  else
    {
      // Register for all events.
      if (0 != this->reactor ()->register_handler
                 (this->pipe_.read_handle (),
                  this,
                  ACE_Event_Handler::READ_MASK | ACE_Event_Handler::WRITE_MASK))
        ACE_ERROR ((LM_ERROR, ACE_TEXT ("%p\n"), ACE_TEXT ("register")));
      else
        this->ok_ = true;
    }
}


Handler::~Handler (void)
{
  this->pipe_.close ();
}


ACE_HANDLE
Handler::get_handle (void) const
{
  return this->pipe_.read_handle ();
}

int
Handler::handle_timeout (const ACE_Time_Value &,
                         const void *)
{
  int const me = this->dispatch_order_++;
  if (me != 1)
    ACE_ERROR ((LM_ERROR,
                ACE_TEXT ("handle_timeout should be #1; it's %d\n"),
                me));
  else
    ACE_DEBUG ((LM_DEBUG, ACE_TEXT ("Handler::handle_timeout\n")));

  return 0;
}

int
Handler::handle_output (ACE_HANDLE)
{
  int const me = this->dispatch_order_++;
  if (me != 2)
    ACE_ERROR ((LM_ERROR,
                ACE_TEXT ("handle_output should be #2; it's %d\n"),
                me));
  else
    ACE_DEBUG ((LM_DEBUG, ACE_TEXT ("Handler::handle_output\n")));

#if defined (__OpenBSD__) || defined (ACE_VXWORKS) || defined (__Lynx__)
  // All that we need written has been written, so don't
  // call handle_output again.
  this->reactor ()->mask_ops (this->pipe_.read_handle (),
                              ACE_Event_Handler::WRITE_MASK,
                              ACE_Reactor::CLR_MASK);
#endif /* __OpenBSD__ || ACE_VXWORKS || __Lynx__ */

  return 0;
}

int
Handler::handle_input (ACE_HANDLE fd)
{
  int const me = this->dispatch_order_++;
  if (me != 3)
    ACE_ERROR ((LM_ERROR,
                ACE_TEXT ("handle_input should be #3; it's %d\n"),
                me));

  char buffer[BUFSIZ];
  ssize_t result = ACE::recv (fd, buffer, sizeof buffer);
  if (result != ssize_t (ACE_OS::strlen (message)))
    ACE_ERROR ((LM_ERROR, ACE_TEXT ("Handler recv'd %b bytes; expected %B\n"),
                result, ACE_OS::strlen (message)));
  buffer[result] = '\0';

  ACE_DEBUG ((LM_DEBUG, ACE_TEXT ("Handler::handle_input: %C\n"), buffer));

  if (ACE_OS::strcmp (buffer, message) != 0)
    ACE_ERROR ((LM_ERROR,
                ACE_TEXT ("Handler text mismatch; received \"%C\"; ")
                ACE_TEXT ("expected \"%C\"\n"),
                buffer, message));

  this->reactor ()->end_reactor_event_loop ();

  return 0;
}

static bool
test_reactor_dispatch_order (ACE_Reactor &reactor)
{
  Handler handler (reactor);
  if (!handler.ok_)
    {
      ACE_ERROR ((LM_ERROR, ACE_TEXT ("Error initializing test; abort.\n")));
      return false;
    }

  bool ok_to_go = true;

  // This should trigger a call to <handle_input>.
  ssize_t result =
    ACE::send_n (handler.pipe_.write_handle (),
                 message,
                 ACE_OS::strlen (message));
  if (result != ssize_t (ACE_OS::strlen (message)))
    {
      ACE_ERROR ((LM_ERROR, ACE_TEXT ("Handler sent %b bytes; should be %B\n"),
                  result, ACE_OS::strlen (message)));
      ok_to_go = false;
    }

  // This should trigger a call to <handle_timeout>.
  if (-1 == reactor.schedule_timer (&handler,
                                    0,
                                    ACE_Time_Value (0)))
    {
      ACE_ERROR ((LM_ERROR, ACE_TEXT ("%p\n"), ACE_TEXT ("schedule_timer")));
      ok_to_go = false;
    }

  // Suspend the handlers - only the timer should be dispatched
  ACE_Time_Value tv (1);
  reactor.suspend_handlers ();
  if (0 != reactor.run_reactor_event_loop (tv))
    {
      ACE_ERROR ((LM_ERROR,
                  ACE_TEXT ("%p\n"),
                  ACE_TEXT ("run_reactor_event_loop")));
      ok_to_go = false;
    }

  // only the timer should have fired
  if (handler.dispatch_order_ != 2)
    {
      ACE_ERROR ((LM_ERROR,
                  ACE_TEXT ("Incorrect number fired %d; expected 2\n"),
                  handler.dispatch_order_));
      ok_to_go = false;
    }

  // Reset the dispatch_order_ count and schedule another timer
  handler.dispatch_order_ = 1;
  if (-1 == reactor.schedule_timer (&handler,
                                    0,
                                    ACE_Time_Value (0)))
    {
      ACE_ERROR ((LM_ERROR, ACE_TEXT ("%p\n"), ACE_TEXT ("schedule_timer")));
      ok_to_go = false;
    }

  // Resume the handlers - things should work now
  reactor.resume_handlers ();

  if (ok_to_go)
    {
      if (0 != reactor.run_reactor_event_loop (tv))
        {
          ACE_ERROR ((LM_ERROR,
                      ACE_TEXT ("%p\n"),
                      ACE_TEXT ("run_reactor_event_loop 2")));
          ok_to_go = false;
        }
    }

  if (0 != reactor.remove_handler (handler.pipe_.read_handle (),
                                   ACE_Event_Handler::ALL_EVENTS_MASK |
                                   ACE_Event_Handler::DONT_CALL))
    ACE_ERROR ((LM_ERROR,
                ACE_TEXT ("%p\n"),
                ACE_TEXT ("remover_handler pipe")));

  if (handler.dispatch_order_ != 4)
    {
      ACE_ERROR ((LM_ERROR,
                  ACE_TEXT ("Incorrect number fired %d; expected 4\n"),
                  handler.dispatch_order_));
      ok_to_go = false;
    }

  int nr_cancelled = reactor.cancel_timer (&handler);
  if (nr_cancelled > 0)
    ACE_ERROR ((LM_ERROR,
                ACE_TEXT ("Finishing test with %d timers still scheduled\n"),
                nr_cancelled));
  return ok_to_go;
}

int
run_main (int, ACE_TCHAR *[])
{
  ACE_START_TEST (ACE_TEXT ("Reactor_Dispatch_Order_Test"));

  int result = 0;
  ACE_Select_Reactor select_reactor_impl;
  ACE_Reactor select_reactor (&select_reactor_impl);
  ACE_DEBUG ((LM_DEBUG, ACE_TEXT ("Testing ACE_Select_Reactor\n")));
  if (!test_reactor_dispatch_order (select_reactor))
    ++result;

  // Winsock 2 things are needed for WFMO_Reactor.
#if defined (ACE_WIN32) && \
    (defined (ACE_HAS_WINSOCK2) && (ACE_HAS_WINSOCK2 != 0))

  ACE_WFMO_Reactor wfmo_reactor_impl;
  ACE_Reactor wfmo_reactor (&wfmo_reactor_impl);
  ACE_DEBUG ((LM_DEBUG, ACE_TEXT ("Testing ACE_WFMO_Reactor\n")));
  if (!test_reactor_dispatch_order (wfmo_reactor))
    ++result;

#endif /* ACE_WIN32 && ACE_HAS_WINSOCK2 */

  ACE_END_TEST;
  return result;
}
