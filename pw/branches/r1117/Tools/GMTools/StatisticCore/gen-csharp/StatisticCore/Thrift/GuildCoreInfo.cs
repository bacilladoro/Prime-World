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

namespace StatisticCore.Thrift
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class GuildCoreInfo : TBase
  {
    private long _guildid;
    private long _auid;
    private int _timestamp;
    private string _faction;
    private int _guildmembers;

    public long Guildid
    {
      get
      {
        return _guildid;
      }
      set
      {
        __isset.guildid = true;
        this._guildid = value;
      }
    }

    public long Auid
    {
      get
      {
        return _auid;
      }
      set
      {
        __isset.auid = true;
        this._auid = value;
      }
    }

    public int Timestamp
    {
      get
      {
        return _timestamp;
      }
      set
      {
        __isset.timestamp = true;
        this._timestamp = value;
      }
    }

    public string Faction
    {
      get
      {
        return _faction;
      }
      set
      {
        __isset.faction = true;
        this._faction = value;
      }
    }

    public int Guildmembers
    {
      get
      {
        return _guildmembers;
      }
      set
      {
        __isset.guildmembers = true;
        this._guildmembers = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool guildid;
      public bool auid;
      public bool timestamp;
      public bool faction;
      public bool guildmembers;
    }

    public GuildCoreInfo() {
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
            if (field.Type == TType.I64) {
              Guildid = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              Auid = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Timestamp = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              Faction = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              Guildmembers = iprot.ReadI32();
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
      TStruct struc = new TStruct("GuildCoreInfo");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.guildid) {
        field.Name = "guildid";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Guildid);
        oprot.WriteFieldEnd();
      }
      if (__isset.auid) {
        field.Name = "auid";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Auid);
        oprot.WriteFieldEnd();
      }
      if (__isset.timestamp) {
        field.Name = "timestamp";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Timestamp);
        oprot.WriteFieldEnd();
      }
      if (Faction != null && __isset.faction) {
        field.Name = "faction";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Faction);
        oprot.WriteFieldEnd();
      }
      if (__isset.guildmembers) {
        field.Name = "guildmembers";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Guildmembers);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("GuildCoreInfo(");
      sb.Append("Guildid: ");
      sb.Append(Guildid);
      sb.Append(",Auid: ");
      sb.Append(Auid);
      sb.Append(",Timestamp: ");
      sb.Append(Timestamp);
      sb.Append(",Faction: ");
      sb.Append(Faction);
      sb.Append(",Guildmembers: ");
      sb.Append(Guildmembers);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
