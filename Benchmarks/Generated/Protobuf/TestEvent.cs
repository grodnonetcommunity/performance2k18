// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: TestEvent.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Testevents.Protobuf {

  /// <summary>Holder for reflection information generated from TestEvent.proto</summary>
  public static partial class TestEventReflection {

    #region Descriptor
    /// <summary>File descriptor for TestEvent.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static TestEventReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg9UZXN0RXZlbnQucHJvdG8SE3Rlc3RldmVudHMucHJvdG9idWYaH2dvb2ds",
            "ZS9wcm90b2J1Zi90aW1lc3RhbXAucHJvdG8idQoJVGVzdEV2ZW50EgoKAmlk",
            "GAEgASgMEgwKBG5hbWUYAiABKAkSCQoBeBgDIAEoARIJCgF5GAQgASgBEgkK",
            "AXoYBSABKAESLQoJdGltZVN0YW1wGAYgASgLMhouZ29vZ2xlLnByb3RvYnVm",
            "LlRpbWVzdGFtcGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Testevents.Protobuf.TestEvent), global::Testevents.Protobuf.TestEvent.Parser, new[]{ "Id", "Name", "X", "Y", "Z", "TimeStamp" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class TestEvent : pb::IMessage<TestEvent> {
    private static readonly pb::MessageParser<TestEvent> _parser = new pb::MessageParser<TestEvent>(() => new TestEvent());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<TestEvent> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Testevents.Protobuf.TestEventReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TestEvent() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TestEvent(TestEvent other) : this() {
      id_ = other.id_;
      name_ = other.name_;
      x_ = other.x_;
      y_ = other.y_;
      z_ = other.z_;
      timeStamp_ = other.timeStamp_ != null ? other.timeStamp_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TestEvent Clone() {
      return new TestEvent(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private pb::ByteString id_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Id {
      get { return id_; }
      set {
        id_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 2;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 3;
    private double x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 4;
    private double y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    /// <summary>Field number for the "z" field.</summary>
    public const int ZFieldNumber = 5;
    private double z_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double Z {
      get { return z_; }
      set {
        z_ = value;
      }
    }

    /// <summary>Field number for the "timeStamp" field.</summary>
    public const int TimeStampFieldNumber = 6;
    private global::Google.Protobuf.WellKnownTypes.Timestamp timeStamp_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.WellKnownTypes.Timestamp TimeStamp {
      get { return timeStamp_; }
      set {
        timeStamp_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as TestEvent);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(TestEvent other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Name != other.Name) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(X, other.X)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Y, other.Y)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Z, other.Z)) return false;
      if (!object.Equals(TimeStamp, other.TimeStamp)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id.Length != 0) hash ^= Id.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (X != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(X);
      if (Y != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Y);
      if (Z != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Z);
      if (timeStamp_ != null) hash ^= TimeStamp.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id.Length != 0) {
        output.WriteRawTag(10);
        output.WriteBytes(Id);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (X != 0D) {
        output.WriteRawTag(25);
        output.WriteDouble(X);
      }
      if (Y != 0D) {
        output.WriteRawTag(33);
        output.WriteDouble(Y);
      }
      if (Z != 0D) {
        output.WriteRawTag(41);
        output.WriteDouble(Z);
      }
      if (timeStamp_ != null) {
        output.WriteRawTag(50);
        output.WriteMessage(TimeStamp);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Id);
      }
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (X != 0D) {
        size += 1 + 8;
      }
      if (Y != 0D) {
        size += 1 + 8;
      }
      if (Z != 0D) {
        size += 1 + 8;
      }
      if (timeStamp_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(TimeStamp);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(TestEvent other) {
      if (other == null) {
        return;
      }
      if (other.Id.Length != 0) {
        Id = other.Id;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.X != 0D) {
        X = other.X;
      }
      if (other.Y != 0D) {
        Y = other.Y;
      }
      if (other.Z != 0D) {
        Z = other.Z;
      }
      if (other.timeStamp_ != null) {
        if (timeStamp_ == null) {
          timeStamp_ = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        TimeStamp.MergeFrom(other.TimeStamp);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Id = input.ReadBytes();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
          case 25: {
            X = input.ReadDouble();
            break;
          }
          case 33: {
            Y = input.ReadDouble();
            break;
          }
          case 41: {
            Z = input.ReadDouble();
            break;
          }
          case 50: {
            if (timeStamp_ == null) {
              timeStamp_ = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(timeStamp_);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
