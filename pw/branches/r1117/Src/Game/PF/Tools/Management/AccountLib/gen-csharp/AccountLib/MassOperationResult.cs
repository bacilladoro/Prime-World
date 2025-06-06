/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace AccountLib
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class MassOperationResult : TBase
  {
    private int _operationId;
    private MassOperationType _type;
    private int _startTime;
    private int _endTime;
    private string _failedAuids;
    private bool _finished;

    public int OperationId
    {
      get
      {
        return _operationId;
      }
      set
      {
        __isset.operationId = true;
        this._operationId = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="MassOperationType"/>
    /// </summary>
    public MassOperationType Type
    {
      get
      {
        return _type;
      }
      set
      {
        __isset.type = true;
        this._type = value;
      }
    }

    public int StartTime
    {
      get
      {
        return _startTime;
      }
      set
      {
        __isset.startTime = true;
        this._startTime = value;
      }
    }

    public int EndTime
    {
      get
      {
        return _endTime;
      }
      set
      {
        __isset.endTime = true;
        this._endTime = value;
      }
    }

    public string FailedAuids
    {
      get
      {
        return _failedAuids;
      }
      set
      {
        __isset.failedAuids = true;
        this._failedAuids = value;
      }
    }

    public bool Finished
    {
      get
      {
        return _finished;
      }
      set
      {
        __isset.finished = true;
        this._finished = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool operationId;
      public bool type;
      public bool startTime;
      public bool endTime;
      public bool failedAuids;
      public bool finished;
    }

    public MassOperationResult() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I32) {
              OperationId = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              Type = (MassOperationType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              StartTime = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              EndTime = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              FailedAuids = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Bool) {
              Finished = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("MassOperationResult");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.operationId) {
        field.Name = "operationId";
        field.Type = TType.I32;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(OperationId);
        oprot.WriteFieldEnd();
      }
      if (__isset.type) {
        field.Name = "type";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Type);
        oprot.WriteFieldEnd();
      }
      if (__isset.startTime) {
        field.Name = "startTime";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(StartTime);
        oprot.WriteFieldEnd();
      }
      if (__isset.endTime) {
        field.Name = "endTime";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(EndTime);
        oprot.WriteFieldEnd();
      }
      if (FailedAuids != null && __isset.failedAuids) {
        field.Name = "failedAuids";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(FailedAuids);
        oprot.WriteFieldEnd();
      }
      if (__isset.finished) {
        field.Name = "finished";
        field.Type = TType.Bool;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(Finished);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("MassOperationResult(");
      sb.Append("OperationId: ");
      sb.Append(OperationId);
      sb.Append(",Type: ");
      sb.Append(Type);
      sb.Append(",StartTime: ");
      sb.Append(StartTime);
      sb.Append(",EndTime: ");
      sb.Append(EndTime);
      sb.Append(",FailedAuids: ");
      sb.Append(FailedAuids);
      sb.Append(",Finished: ");
      sb.Append(Finished);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
