using ProtoBuf;

using System.Collections.Generic;
using Fantasy;
using Fantasy.Network.Interface;
using Fantasy.Serialize;
#pragma warning disable CS8618

namespace Fantasy
{
	[ProtoContract]
	public partial class Send_Test1 : AMessage, IRequest, IProto
	{
		public static Send_Test1 Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_Test1>();
		}
		public override void Dispose()
		{
			user_name = default;
			pass_word = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_Test1>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_Test1 ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_Test1; }
		[ProtoMember(1)]
		public string user_name { get; set; }
		[ProtoMember(2)]
		public string pass_word { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_Test1 : AMessage, IResponse, IProto
	{
		public static Rcv_Test1 Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_Test1>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			success = default;
			error_msg = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_Test1>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_Test1; }
		[ProtoMember(1)]
		public bool success { get; set; }
		[ProtoMember(2)]
		public string error_msg { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
}
