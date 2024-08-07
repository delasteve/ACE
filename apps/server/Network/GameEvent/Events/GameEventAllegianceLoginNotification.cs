using System;

namespace ACE.Server.Network.GameEvent.Events;

class GameEventAllegianceLoginNotification : GameEventMessage
{
    public GameEventAllegianceLoginNotification(Session session, uint playerGuid, bool isLoggedIn)
        : base(GameEventType.AllegianceLoginNotification, GameMessageGroup.UIQueue, session, 12)
    {
        Writer.Write(playerGuid);
        Writer.Write(Convert.ToUInt32(isLoggedIn));
    }
}
