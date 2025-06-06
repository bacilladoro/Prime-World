package MakeWorkspaceBase;

# ************************************************************
# Description   : A Make Workspace base module
# Author        : Chad Elliott
# Create Date   : 11/21/2006
# ************************************************************

# ************************************************************
# Pragmas
# ************************************************************

use strict;

# ************************************************************
# Subroutine Section
# ************************************************************

sub targets {
  return $_[0]->{'make_targets'};
}

sub workspace_file_prefix {
  #my $self = shift;
  return 'Makefile';
}


sub workspace_file_extension {
  #my $self = shift;
  return '';
}


sub supports_make_coexistence {
  return ($_[0]->workspace_file_extension() ne '');
}


sub workspace_file_name {
  my $self = shift;
  return $self->get_modified_workspace_name(
                                 $self->workspace_file_prefix(),
                                 $self->make_coexistence() ?
                                     $self->workspace_file_extension() : '');
}


sub workspace_per_project {
  #my $self = shift;
  return 1;
}


sub workspace_preamble {
  my($self, $fh, $crlf, $name, $id) = @_;

  ## Optionally print the workspace comment
  $self->print_workspace_comment($fh,
            '#----------------------------------------------------------------------------', $crlf,
            '#       ', $name, $crlf,
            '#', $crlf,
            '# ', $id, $crlf,
            '#', $crlf,
            '# This file was generated by MPC.  Any changes made directly to', $crlf,
            '# this file will be lost the next time it is generated.', $crlf,
            '#', $crlf,
            '# MPC Command:', $crlf,
            '# ', $self->create_command_line_string($0, @ARGV), $crlf,
            '#', $crlf,
            '#----------------------------------------------------------------------------', $crlf,
            $crlf);
}


sub write_named_targets {
  my($self, $fh, $crlf, $targnum, $list, $remain, $targpre, $allpre, $trans, $phony, $andsym, $maxline) = @_;

  ## Save the targets for later
  $self->{'make_targets'} = $remain;

  ## Print out the "all" target
  if (defined $maxline) {
    my $all = 'all:';
    foreach my $project (@$list) {
      $all .= " $$trans{$project}";
    }
    if (length($all) < $maxline) {
      print $fh $crlf, $all;
    }
    else {
      $remain = 'all ' . $remain;
    }
  }
  else {
    print $fh $crlf . 'all:';
    foreach my $project (@$list) {
      print $fh " $$trans{$project}";
    }
  }

  ## Print out all other targets here
  print $fh "$crlf$crlf$remain:$crlf";
  $self->write_project_targets($fh, $crlf,
                               $targpre . '$(@)', $list, $andsym);

  ## Print out each target separately
  foreach my $project (@$list) {
    print $fh ($phony ? "$crlf.PHONY: $$trans{$project}" : ''),
              $crlf, $$trans{$project}, ':';
    if (defined $$targnum{$project}) {
      foreach my $number (@{$$targnum{$project}}) {
        print $fh " $$trans{$$list[$number]}";
      }
    }
    print $fh $crlf;
    $self->write_project_targets($fh, $crlf,
                                 $targpre . $allpre . 'all',
                                 [ $project ], $andsym);
  }

  ## Print out the project_name_list target
  print $fh $crlf, "project_name_list:$crlf";
  foreach my $project (sort @$list) {
    print $fh "\t\@echo $$trans{$project}$crlf";
  }
}


sub post_workspace {
  my($self, $wsfh, $creator, $toplevel) = @_;

  if ($toplevel && $self->{'for_eclipse'}) {
    my $crlf    = $self->crlf();
    my $outdir  = $self->get_outdir();
    my $fh      = new FileHandle();
    my $outfile = "$outdir/.cdtproject";
    my $pjt     = $self->get_eclipse_cdtproject();

    if (open($fh, ">$outfile")) {
      ## We want to set the make command to nmake for the nmake project
      ## type.  As far as stopping on an error, I don't remember why this
      ## is true only for Borland make.
      my $cmd = ("$self" =~ /^nmake/i ? 'nmake' : 'make');
      my $stop = ("$self" =~ /^bmake/i ? 'true' : 'false');
      print $fh $$pjt[0];
      foreach my $target ('all',
                          grep(/^[\w\-]+$/, split(/\s+/, $self->targets()))) {
        print $fh '      <target name="', $target, '" path="" targetID="org.eclipse.cdt.make.MakeTargetBuilder">', $crlf,
                  '        <buildCommand>', $cmd, '</buildCommand>', $crlf,
                  '        <buildArguments></buildArguments>', $crlf,
                  '        <buildTarget>', $target, '</buildTarget>  ', $crlf,
                  '        <stopOnError>', $stop, '</stopOnError>', $crlf,
                  '        <useDefaultCommand>false</useDefaultCommand>', $crlf,
                  '      </target>', $crlf;
      }
      print $fh $$pjt[1];
      close($fh);
    }
    else {
      $self->warning("Unable to create $outfile");
    }

    ## Create the eclipse project which is unchanging except for the name
    ## of the starting makefile.
    $pjt = $self->get_eclipse_project();
    $outfile = "$outdir/.project";
    if (open($fh, ">$outfile")) {
      print $fh $$pjt[0], $self->get_workspace_name(), $$pjt[1];
      close($fh);
    }
    else {
      $self->warning("Unable to create $outfile");
    }
  }
}


sub get_eclipse_cdtproject {
  my $self = shift;
  if (!defined $self->{'eclipse_cdtproject'}) {
    $self->{'eclipse_cdtproject'} = [
'<?xml version="1.0" encoding="UTF-8"?>
<?eclipse-cdt version="2.0"?>
<cdtproject id="org.eclipse.cdt.make.core.make">
  <extension id="org.eclipse.cdt.core.ELF" point="org.eclipse.cdt.core.BinaryParser"/>
  <extension id="org.eclipse.cdt.core.nullindexer" point="org.eclipse.cdt.core.CIndexer"/>
  <data>
    <item id="scannerConfiguration">
      <autodiscovery enabled="true" problemReportingEnabled="true" selectedProfileId="org.eclipse.cdt.make.core.GCCStandardMakePerProjectProfile"/>
      <profile id="org.eclipse.cdt.make.core.GCCStandardMakePerProjectProfile">
        <buildOutputProvider>
          <openAction enabled="true" filePath=""/>
          <parser enabled="true"/>
        </buildOutputProvider>
      </profile>
      <profile id="org.eclipse.cdt.managedbuilder.core.GCCManagedMakePerProjectProfile">
        <buildOutputProvider>
          <openAction enabled="false" filePath=""/>
          <parser enabled="false"/>
        </buildOutputProvider>
        <scannerInfoProvider id="specsFile">
          <runAction arguments="-E -P -v -dD ${plugin_state_location}/${specs_file}" command="gcc" useDefault="true"/>
          <parser enabled="false"/>
        </scannerInfoProvider>
      </profile>
      <profile id="org.eclipse.cdt.managedbuilder.core.GCCWinManagedMakePerProjectProfile">
        <buildOutputProvider>
          <openAction enabled="false" filePath=""/>
          <parser enabled="false"/>
        </buildOutputProvider>
        <scannerInfoProvider id="specsFile">
          <runAction arguments="-E -P -v -dD ${plugin_state_location}/${specs_file}" command="gcc" useDefault="true"/>
          <parser enabled="false"/>
        </scannerInfoProvider>
      </profile>
      <profile id="org.eclipse.cdt.make.core.GCCStandardMakePerFileProfile">
        <buildOutputProvider>
         <openAction enabled="false" filePath=""/>
          <parser enabled="false"/>
        </buildOutputProvider>
        <scannerInfoProvider id="makefileGenerator">
          <runAction arguments="-f ${project_name}_scd.mk" command="make" useDefault="true"/>
          <parser enabled="false"/>
        </scannerInfoProvider>
      </profile>
    </item>
      <item id="org.eclipse.cdt.core.pathentry">
      <pathentry kind="src" path=""/>
      <pathentry kind="out" path=""/>
      <pathentry kind="con" path="org.eclipse.cdt.make.core.DISCOVERED_SCANNER_INFO"/>
    </item>
    <item id="org.eclipse.cdt.make.core.buildtargets">
      <buildTargets>
',

'      </buildTargets>
    </item>
  </data>
</cdtproject>
'];
  }
  return $self->{'eclipse_cdtproject'};
}


sub get_eclipse_project {
  my $self = shift;
  if (!defined $self->{'eclipse_project'}) {
    $self->{'eclipse_project'} = [
'<?xml version="1.0" encoding="UTF-8"?>
<projectDescription>
  <name>',
'</name>
  <comment></comment>
  <projects>
  </projects>
  <buildSpec>
    <buildCommand>
      <name>org.eclipse.cdt.make.core.makeBuilder</name>
      <triggers>clean,full,incremental,</triggers>
      <arguments>
        <dictionary>
          <key>org.eclipse.cdt.make.core.build.arguments</key>
          <value></value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.core.errorOutputParser</key>
          <value>org.eclipse.cdt.core.MakeErrorParser;org.eclipse.cdt.core.GCCErrorParser;org.eclipse.cdt.core.GASErrorParser;org.eclipse.cdt.core.GLDErrorParser;org.eclipse.cdt.core.VCErrorParser;</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.enableAutoBuild</key>
          <value>false</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.environment</key>
          <value></value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.enableFullBuild</key>
          <value>true</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.build.target.inc</key>
          <value>all</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.enabledIncrementalBuild</key>
          <value>true</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.build.target.clean</key>
          <value>clean</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.build.command</key>
          <value>make</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.enableCleanBuild</key>
          <value>true</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.append_environment</key>
          <value>true</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.build.target.full</key>
          <value>clean all</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.useDefaultBuildCmd</key>
          <value>true</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.build.target.auto</key>
          <value>all</value>
        </dictionary>
        <dictionary>
          <key>org.eclipse.cdt.make.core.stopOnError</key>
          <value>false</value>
        </dictionary>
      </arguments>
    </buildCommand>
    <buildCommand>
      <name>org.eclipse.cdt.make.core.ScannerConfigBuilder</name>
      <arguments>
      </arguments>
    </buildCommand>
  </buildSpec>
  <natures>
    <nature>org.eclipse.cdt.core.cnature</nature>
    <nature>org.eclipse.cdt.make.core.makeNature</nature>
    <nature>org.eclipse.cdt.make.core.ScannerConfigNature</nature>
    <nature>org.eclipse.cdt.core.ccnature</nature>
  </natures>
</projectDescription>
'];
  }
  return $self->{'eclipse_project'};
}

1;
