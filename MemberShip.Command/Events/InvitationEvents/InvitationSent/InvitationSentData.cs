﻿namespace MemberShip.Command.Events.InvitationEvents.InvitationSent;
public record InvitationSentData(string AccountId, string SubscriptionId, string MemberId, string UserId);