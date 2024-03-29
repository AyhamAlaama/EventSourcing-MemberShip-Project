﻿namespace MemberShip.Query.EventHandler;

public record class UnknownEvent(
    string AggregateId,
    int Sequence,
    DateTime DateTime,
    string UserID,
    int Version
) : Event<object>(
    AggregateId: AggregateId,
    Sequence: Sequence,
    Data: new(),
    DateTime: DateTime,
    UserId: UserID,
    Version: Version
);
