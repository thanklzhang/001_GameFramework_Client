// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: BattleEntrance.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace NetProto {

  /// <summary>Holder for reflection information generated from BattleEntrance.proto</summary>
  public static partial class BattleEntranceReflection {

    #region Descriptor
    /// <summary>File descriptor for BattleEntrance.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BattleEntranceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRCYXR0bGVFbnRyYW5jZS5wcm90bxIITmV0UHJvdG8aDENvbW1vbi5wcm90",
            "bxoMQmF0dGxlLnByb3RvIhcKFWNzQXBwbHlIZXJvRXhhbUJhdHRsZSIkChVz",
            "Y0FwcGx5SGVyb0V4YW1CYXR0bGUSCwoDZXJyGAEgASgFIjsKFWNzQXBwbHlN",
            "YWluVGFza0JhdHRsZRIRCgljaGFwdGVySWQYASABKAUSDwoHc3RhZ2VJZBgC",
            "IAEoBSJIChVzY0FwcGx5TWFpblRhc2tCYXR0bGUSCwoDZXJyGAEgASgFEhEK",
            "CWNoYXB0ZXJJZBgCIAEoBRIPCgdzdGFnZUlkGAMgASgFYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::NetProto.CommonReflection.Descriptor, global::NetProto.BattleReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::NetProto.csApplyHeroExamBattle), global::NetProto.csApplyHeroExamBattle.Parser, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::NetProto.scApplyHeroExamBattle), global::NetProto.scApplyHeroExamBattle.Parser, new[]{ "Err" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::NetProto.csApplyMainTaskBattle), global::NetProto.csApplyMainTaskBattle.Parser, new[]{ "ChapterId", "StageId" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::NetProto.scApplyMainTaskBattle), global::NetProto.scApplyMainTaskBattle.Parser, new[]{ "Err", "ChapterId", "StageId" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// 申请进入英雄试炼战斗
  /// </summary>
  public sealed partial class csApplyHeroExamBattle : pb::IMessage<csApplyHeroExamBattle> {
    private static readonly pb::MessageParser<csApplyHeroExamBattle> _parser = new pb::MessageParser<csApplyHeroExamBattle>(() => new csApplyHeroExamBattle());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<csApplyHeroExamBattle> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::NetProto.BattleEntranceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public csApplyHeroExamBattle() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public csApplyHeroExamBattle(csApplyHeroExamBattle other) : this() {
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public csApplyHeroExamBattle Clone() {
      return new csApplyHeroExamBattle(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as csApplyHeroExamBattle);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(csApplyHeroExamBattle other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(csApplyHeroExamBattle other) {
      if (other == null) {
        return;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
        }
      }
    }

  }

  public sealed partial class scApplyHeroExamBattle : pb::IMessage<scApplyHeroExamBattle> {
    private static readonly pb::MessageParser<scApplyHeroExamBattle> _parser = new pb::MessageParser<scApplyHeroExamBattle>(() => new scApplyHeroExamBattle());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<scApplyHeroExamBattle> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::NetProto.BattleEntranceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public scApplyHeroExamBattle() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public scApplyHeroExamBattle(scApplyHeroExamBattle other) : this() {
      err_ = other.err_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public scApplyHeroExamBattle Clone() {
      return new scApplyHeroExamBattle(this);
    }

    /// <summary>Field number for the "err" field.</summary>
    public const int ErrFieldNumber = 1;
    private int err_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Err {
      get { return err_; }
      set {
        err_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as scApplyHeroExamBattle);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(scApplyHeroExamBattle other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Err != other.Err) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Err != 0) hash ^= Err.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Err != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Err);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Err != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Err);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(scApplyHeroExamBattle other) {
      if (other == null) {
        return;
      }
      if (other.Err != 0) {
        Err = other.Err;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Err = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// 申请进入主线战斗
  /// </summary>
  public sealed partial class csApplyMainTaskBattle : pb::IMessage<csApplyMainTaskBattle> {
    private static readonly pb::MessageParser<csApplyMainTaskBattle> _parser = new pb::MessageParser<csApplyMainTaskBattle>(() => new csApplyMainTaskBattle());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<csApplyMainTaskBattle> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::NetProto.BattleEntranceReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public csApplyMainTaskBattle() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public csApplyMainTaskBattle(csApplyMainTaskBattle other) : this() {
      chapterId_ = other.chapterId_;
      stageId_ = other.stageId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public csApplyMainTaskBattle Clone() {
      return new csApplyMainTaskBattle(this);
    }

    /// <summary>Field number for the "chapterId" field.</summary>
    public const int ChapterIdFieldNumber = 1;
    private int chapterId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ChapterId {
      get { return chapterId_; }
      set {
        chapterId_ = value;
      }
    }

    /// <summary>Field number for the "stageId" field.</summary>
    public const int StageIdFieldNumber = 2;
    private int stageId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int StageId {
      get { return stageId_; }
      set {
        stageId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as csApplyMainTaskBattle);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(csApplyMainTaskBattle other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ChapterId != other.ChapterId) return false;
      if (StageId != other.StageId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ChapterId != 0) hash ^= ChapterId.GetHashCode();
      if (StageId != 0) hash ^= StageId.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ChapterId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ChapterId);
      }
      if (StageId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(StageId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ChapterId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ChapterId);
      }
      if (StageId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(StageId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(csApplyMainTaskBattle other) {
      if (other == null) {
        return;
      }
      if (other.ChapterId != 0) {
        ChapterId = other.ChapterId;
      }
      if (other.StageId != 0) {
        StageId = other.StageId;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            ChapterId = input.ReadInt32();
            break;
          }
          case 16: {
            StageId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class scApplyMainTaskBattle : pb::IMessage<scApplyMainTaskBattle> {
    private static readonly pb::MessageParser<scApplyMainTaskBattle> _parser = new pb::MessageParser<scApplyMainTaskBattle>(() => new scApplyMainTaskBattle());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<scApplyMainTaskBattle> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::NetProto.BattleEntranceReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public scApplyMainTaskBattle() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public scApplyMainTaskBattle(scApplyMainTaskBattle other) : this() {
      err_ = other.err_;
      chapterId_ = other.chapterId_;
      stageId_ = other.stageId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public scApplyMainTaskBattle Clone() {
      return new scApplyMainTaskBattle(this);
    }

    /// <summary>Field number for the "err" field.</summary>
    public const int ErrFieldNumber = 1;
    private int err_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Err {
      get { return err_; }
      set {
        err_ = value;
      }
    }

    /// <summary>Field number for the "chapterId" field.</summary>
    public const int ChapterIdFieldNumber = 2;
    private int chapterId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ChapterId {
      get { return chapterId_; }
      set {
        chapterId_ = value;
      }
    }

    /// <summary>Field number for the "stageId" field.</summary>
    public const int StageIdFieldNumber = 3;
    private int stageId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int StageId {
      get { return stageId_; }
      set {
        stageId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as scApplyMainTaskBattle);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(scApplyMainTaskBattle other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Err != other.Err) return false;
      if (ChapterId != other.ChapterId) return false;
      if (StageId != other.StageId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Err != 0) hash ^= Err.GetHashCode();
      if (ChapterId != 0) hash ^= ChapterId.GetHashCode();
      if (StageId != 0) hash ^= StageId.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Err != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Err);
      }
      if (ChapterId != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(ChapterId);
      }
      if (StageId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(StageId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Err != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Err);
      }
      if (ChapterId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ChapterId);
      }
      if (StageId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(StageId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(scApplyMainTaskBattle other) {
      if (other == null) {
        return;
      }
      if (other.Err != 0) {
        Err = other.Err;
      }
      if (other.ChapterId != 0) {
        ChapterId = other.ChapterId;
      }
      if (other.StageId != 0) {
        StageId = other.StageId;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Err = input.ReadInt32();
            break;
          }
          case 16: {
            ChapterId = input.ReadInt32();
            break;
          }
          case 24: {
            StageId = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code