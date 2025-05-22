using ProtoBuf;

using System.Collections.Generic;
using Fantasy;
using Fantasy.Network.Interface;
using Fantasy.Serialize;
#pragma warning disable CS8618

namespace Fantasy
{
	[ProtoContract]
	public partial class Send_RegisterAccount : AMessage, IRequest, IProto
	{
		public static Send_RegisterAccount Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_RegisterAccount>();
		}
		public override void Dispose()
		{
			user_name = default;
			pass_word = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_RegisterAccount>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_RegisterAccount ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_RegisterAccount; }
		[ProtoMember(1)]
		public string user_name { get; set; }
		[ProtoMember(2)]
		public string pass_word { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_RegisterAccount : AMessage, IResponse, IProto
	{
		public static Rcv_RegisterAccount Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_RegisterAccount>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			user_name = default;
			pass_word = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_RegisterAccount>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_RegisterAccount; }
		[ProtoMember(1)]
		public string user_name { get; set; }
		[ProtoMember(2)]
		public string pass_word { get; set; }
		[ProtoMember(3)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Send_GetLoginToken : AMessage, IRequest, IProto
	{
		public static Send_GetLoginToken Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_GetLoginToken>();
		}
		public override void Dispose()
		{
			account_name = default;
			pass_word = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_GetLoginToken>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_GetLoginToken ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_GetLoginToken; }
		[ProtoMember(1)]
		public string account_name { get; set; }
		[ProtoMember(2)]
		public string pass_word { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_GetLoginToken : AMessage, IResponse, IProto
	{
		public static Rcv_GetLoginToken Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_GetLoginToken>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			token = default;
			login_address = default;
			account_id = default;
			scene_config_id = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_GetLoginToken>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_GetLoginToken; }
		[ProtoMember(1)]
		public string token { get; set; }
		[ProtoMember(2)]
		public string login_address { get; set; }
		[ProtoMember(3)]
		public long account_id { get; set; }
		[ProtoMember(4)]
		public uint scene_config_id { get; set; }
		[ProtoMember(5)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Send_LoginGate : AMessage, IRequest, IProto
	{
		public static Send_LoginGate Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Send_LoginGate>();
		}
		public override void Dispose()
		{
			account_id = default;
			token = default;
			scene_config_id = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Send_LoginGate>(this);
#endif
		}
		[ProtoIgnore]
		public Rcv_LoginGate ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.Send_LoginGate; }
		[ProtoMember(1)]
		public long account_id { get; set; }
		[ProtoMember(2)]
		public string token { get; set; }
		[ProtoMember(3)]
		public uint scene_config_id { get; set; }
	}
	[ProtoContract]
	public partial class Rcv_LoginGate : AMessage, IResponse, IProto
	{
		public static Rcv_LoginGate Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Rcv_LoginGate>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			account_id = default;
			level = default;
			gold = default;
			diamonds = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Rcv_LoginGate>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Rcv_LoginGate; }
		[ProtoMember(1)]
		public long account_id { get; set; }
		[ProtoMember(2)]
		public long level { get; set; }
		[ProtoMember(3)]
		public long gold { get; set; }
		[ProtoMember(4)]
		public long diamonds { get; set; }
		[ProtoMember(5)]
		public uint ErrorCode { get; set; }
	}
}
