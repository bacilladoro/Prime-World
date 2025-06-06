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
  public partial class GuildInteractionInfo : TBase
  {
    private long _guildid;
    private long _targetguildid;
    private GuildInteractionType _type;
    private long _siegeid;
    private int _waitingtime;
    private int _timestamp;

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

    public long Targetguildid
    {
      get
      {
        return _targetguildid;
      }
      set
      {
        __isset.targetguildid = true;
        this._targetguildid = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="GuildInteractionType"/>
    /// </summary>
    public GuildInteractionType Type
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

    public long Siegeid
    {
      get
      {
        return _siegeid;
      }
      set
      {
        __isset.siegeid = true;
        this._siegeid = value;
      }
    }

    public int Waitingtime
    {
      get
      {
        return _waitingtime;
      }
      set
      {
        __isset.waitingtime = true;
        this._waitingtime = value;
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


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool guildid;
      public bool targetguildid;
      public bool type;
      public bool siegeid;
      public bool waitingtime;
      public bool timestamp;
    }

    public GuildInteractionInfo() {
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
              Targetguildid = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Type = (GuildInteractionType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I64) {
              Siegeid = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              Waitingtime = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.I32) {
              Timestamp = iprot.ReadI32();
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
      TStruct struc = new TStruct("GuildInteractionInfo");
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
      if (__isset.targetguildid) {
        field.Name = "targetguildid";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Targetguildid);
        oprot.WriteFieldEnd();
      }
      if (__isset.type) {
        field.Name = "type";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Type);
        oprot.WriteFieldEnd();
      }
      if (__isset.siegeid) {
        field.Name = "siegeid";
        field.Type = TType.I64;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Siegeid);
        oprot.WriteFieldEnd();
      }
      if (__isset.waitingtime) {
        field.Name = "waitingtime";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Waitingtime);
        oprot.WriteFieldEnd();
      }
      if (__isset.timestamp) {
        field.Name = "timestamp";
        field.Type = TType.I32;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Timestamp);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("GuildInteractionInfo(");
      sb.Append("Guildid: ");
      sb.Append(Guildid);
      sb.Append(",Targetguildid: ");
      sb.Append(Targetguildid);
      sb.Append(",Type: ");
      sb.Append(Type);
      sb.Append(",Siegeid: ");
      sb.Append(Siegeid);
      sb.Append(",Waitingtime: ");
      sb.Append(Waitingtime);
      sb.Append(",Timestamp: ");
      sb.Append(Timestamp);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
