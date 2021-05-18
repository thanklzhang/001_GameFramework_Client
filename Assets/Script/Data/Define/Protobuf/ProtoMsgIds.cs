//gen by tool
public enum ProtoMsgIds
{
	//CombatProto

	//GC2CS
	//User
	GC2CS_EnterGameService = 13001,//登录成功后 需要请求进入真正的游戏服务 也就是通过 发送到 GS 转发到 CS 验证后即可进入真正的游戏服务
    //Hero
	GC2CS_HeroList = 13101,
	GC2CS_AddHeroLevel = 13102,
	GC2CS_NotifyUpdateHeroes = 13103,
	//Mate
	GC2CS_StartMateCombat = 13130,
	GC2CS_CombatEnd = 13131,

	//GC2LS
	GC2LS_AskLogin = 11001,

	//GC2SS
	GC2SS_SyncCombatInitInfo = 17001,//同步战斗初始信息
	GC2SS_PlayerLoadCombatFinish = 17002,//客户端加载战斗完成
	GC2SS_SyncCombatStart = 17003,//战斗开始
	GC2SS_SyncCombatEnd = 17004,//战斗结束
	GC2SS_NormalAttack = 17005,//普通攻击(作废)
	GC2SS_SyncRoundActionStart = 17006,//回合开始
	GC2SS_CombatPlayerOperation = 17007,//该回合操作完成
	GC2SS_SyncTimelineAllSequence = 17008,//发送所有英雄在这回合的 timeline

	//GS2CS
	GS2CS_UserLogin = 15001,
	GS2CS_UserExit = 15002,
	//GS2CS_FromSS_CreateCombatFinish = 15500,
	GS2CS_FromSS_CombatEnd = 15500,

	//GS2GC
	GS2GC_FromCS_SyncPlayerBaseInfo = 14005,
	//GS2GC_FromCS_StartCombat = 14010,

	//GS2SS
	GS2SS_FromCS_CreateCombat = 16005,

	//HeroProto

	//LineupProto

	//LS2GS
	LS2GS_VerificationAskLogin = 12001,

	//MateProto

	//NetCommon
	HeartBeatHandshake = 101,
	HeartBeatSend = 102,
	HeartBeatBack = 103,

	//ResultCode

	//UserProto

}
