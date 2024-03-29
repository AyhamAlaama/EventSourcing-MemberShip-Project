﻿using MemberShip.Query.EventHandler.Invitation.Accepted;
using MemberShip.Query.EventHandler.Invitation.Canceled;
using MemberShip.Query.EventHandler.Invitation.Rejected;
using MemberShip.Query.EventHandler.Invitation.Sent;

namespace MemberShip.Query.Entities;

public class MemberShip
{
    public string Id { get; private set; }
    public int Sequence { get; private set; }
    public string AccountId { get; private set; }
    public string? SubscriptionId { get; private set; }
    public string MemberId { get; private set; }
    public string UserId { get; private set; }
    public string InviteStatus { get; private set; }
    private MemberShip(
            string id,
            int sequence,
            string accountId,
            string subscriptionId,
            string memberId,
            string userId,
            string inviteStatus)
    {
        Id = id;
        Sequence = sequence;
        AccountId = accountId;
        SubscriptionId = subscriptionId;
        MemberId = memberId;
        UserId = userId;
        InviteStatus = inviteStatus;

    }

    public static MemberShipEntity FromSentEvent(InvitationSent @event) => new
        (
        id: @event.AggregateId,
        sequence: @event.Sequence,
        accountId: @event.Data.AccountId,
        subscriptionId: @event.Data.SubscriptionId,
        memberId: @event.Data.MemberId,
        userId: @event.Data.UserId,
        inviteStatus: "Pending"
        );
    public void InvitationCanceled(InvitationCanceled @event)
    {
        Sequence = @event.Sequence;
        InviteStatus = "Canceled";
    }
    public void NewInvitationSent(InvitationSent @event)
    {
        Sequence = @event.Sequence;
        InviteStatus = "Pending";
    }
    public void InvitationAccepted(InvitationAccepted @event)
    {
        Sequence = @event.Sequence;
        InviteStatus = "Joined";
    }
    public void InvitationRejected(InvitationRejected @event)
    {
        Sequence = @event.Sequence;
        InviteStatus = "Rejected";
    }
    public void IncrementSequence() => Sequence++;
}
