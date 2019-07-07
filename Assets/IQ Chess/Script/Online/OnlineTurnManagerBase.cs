using UnityEngine;
using Photon.Pun;
using System;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using Photon.Realtime;


namespace IQChess.Online
{
	/// <summary>Lớp con phải thông báo sẵn sàng chơi.</summary>
	public abstract class OnlineTurnManagerBase<I, P, T> : TurnManagerBase<I, P> where I : Enum where P : PlayerBase<I, P> where T : OnlineTurnManagerBase<I, P, T>
	{
		#region MonoBehaviourPun
		/// <summary>Cache field for the PhotonView on this GameObject.</summary>
		private PhotonView pvCache;

		/// <summary>A cached reference to a PhotonView on this GameObject.</summary>
		/// <remarks>
		/// If you intend to work with a PhotonView in a script, it's usually easier to write this.photonView.
		///
		/// If you intend to remove the PhotonView component from the GameObject but keep this Photon.MonoBehaviour,
		/// avoid this reference or modify this code to use PhotonView.Get(obj) instead.
		/// </remarks>
		public PhotonView photonView
		{
			get
			{
				if (this.pvCache == null)
				{
					this.pvCache = PhotonView.Get(this);
				}
				return this.pvCache;
			}
		}
		#endregion


		#region MonoBehaviourPunCallbacks
		public virtual void OnEnable()
		{
			PhotonNetwork.AddCallbackTarget(this);
		}

		public virtual void OnDisable()
		{
			PhotonNetwork.RemoveCallbackTarget(this);
		}

		/// <summary>
		/// Called to signal that the raw connection got established but before the client can call operation on the server.
		/// </summary>
		/// <remarks>
		/// After the (low level transport) connection is established, the client will automatically send
		/// the Authentication operation, which needs to get a response before the client can call other operations.
		///
		/// Your logic should wait for either: OnRegionListReceived or OnConnectedToMaster.
		///
		/// This callback is useful to detect if the server can be reached at all (technically).
		/// Most often, it's enough to implement OnDisconnected().
		///
		/// This is not called for transitions from the masterserver to game servers.
		/// </remarks>
		public virtual void OnConnected()
		{
		}

		/// <summary>
		/// Called when the local user/client left a room, so the game's logic can clean up it's internal state.
		/// </summary>
		/// <remarks>
		/// When leaving a room, the LoadBalancingClient will disconnect the Game Server and connect to the Master Server.
		/// This wraps up multiple internal actions.
		///
		/// Wait for the callback OnConnectedToMaster, before you use lobbies and join or create rooms.
		/// </remarks>
		public virtual void OnLeftRoom()
		{
		}

		/// <summary>
		/// Called after switching to a new MasterClient when the current one leaves.
		/// </summary>
		/// <remarks>
		/// This is not called when this client enters a room.
		/// The former MasterClient is still in the player list when this method get called.
		/// </remarks>
		public virtual void OnMasterClientSwitched(Player newMasterClient)
		{
		}

		/// <summary>
		/// Called when the server couldn't create a room (OpCreateRoom failed).
		/// </summary>
		/// <remarks>
		/// The most common cause to fail creating a room, is when a title relies on fixed room-names and the room already exists.
		/// </remarks>
		/// <param name="returnCode">Operation ReturnCode from the server.</param>
		/// <param name="message">Debug message for the error.</param>
		public virtual void OnCreateRoomFailed(short returnCode, string message)
		{
		}

		/// <summary>
		/// Called when a previous OpJoinRoom call failed on the server.
		/// </summary>
		/// <remarks>
		/// The most common causes are that a room is full or does not exist (due to someone else being faster or closing the room).
		/// </remarks>
		/// <param name="returnCode">Operation ReturnCode from the server.</param>
		/// <param name="message">Debug message for the error.</param>
		public virtual void OnJoinRoomFailed(short returnCode, string message)
		{
		}

		/// <summary>
		/// Called when this client created a room and entered it. OnJoinedRoom() will be called as well.
		/// </summary>
		/// <remarks>
		/// This callback is only called on the client which created a room (see OpCreateRoom).
		///
		/// As any client might close (or drop connection) anytime, there is a chance that the
		/// creator of a room does not execute OnCreatedRoom.
		///
		/// If you need specific room properties or a "start signal", implement OnMasterClientSwitched()
		/// and make each new MasterClient check the room's state.
		/// </remarks>
		public virtual void OnCreatedRoom()
		{
		}

		/// <summary>
		/// Called on entering a lobby on the Master Server. The actual room-list updates will call OnRoomListUpdate.
		/// </summary>
		/// <remarks>
		/// While in the lobby, the roomlist is automatically updated in fixed intervals (which you can't modify in the public cloud).
		/// The room list gets available via OnRoomListUpdate.
		/// </remarks>
		public virtual void OnJoinedLobby()
		{
		}

		/// <summary>
		/// Called after leaving a lobby.
		/// </summary>
		/// <remarks>
		/// When you leave a lobby, [OpCreateRoom](@ref OpCreateRoom) and [OpJoinRandomRoom](@ref OpJoinRandomRoom)
		/// automatically refer to the default lobby.
		/// </remarks>
		public virtual void OnLeftLobby()
		{
		}

		/// <summary>
		/// Called after disconnecting from the Photon server. It could be a failure or intentional
		/// </summary>
		/// <remarks>
		/// The reason for this disconnect is provided as DisconnectCause.
		/// </remarks>
		public virtual void OnDisconnected(DisconnectCause cause)
		{
		}

		/// <summary>
		/// Called when the Name Server provided a list of regions for your title.
		/// </summary>
		/// <remarks>Check the RegionHandler class description, to make use of the provided values.</remarks>
		/// <param name="regionHandler">The currently used RegionHandler.</param>
		public virtual void OnRegionListReceived(RegionHandler regionHandler)
		{
		}

		/// <summary>
		/// Called for any update of the room-listing while in a lobby (InLobby) on the Master Server.
		/// </summary>
		/// <remarks>
		/// Each item is a RoomInfo which might include custom properties (provided you defined those as lobby-listed when creating a room).
		/// Not all types of lobbies provide a listing of rooms to the client. Some are silent and specialized for server-side matchmaking.
		/// </remarks>
		public virtual void OnRoomListUpdate(List<RoomInfo> roomList)
		{
		}

		/// <summary>
		/// Called when the LoadBalancingClient entered a room, no matter if this client created it or simply joined.
		/// </summary>
		/// <remarks>
		/// When this is called, you can access the existing players in Room.Players, their custom properties and Room.CustomProperties.
		///
		/// In this callback, you could create player objects. For example in Unity, instantiate a prefab for the player.
		///
		/// If you want a match to be started "actively", enable the user to signal "ready" (using OpRaiseEvent or a Custom Property).
		/// </remarks>
		public virtual void OnJoinedRoom()
		{
		}

		/// <summary>
		/// Called when a remote player entered the room. This Player is already added to the playerlist.
		/// </summary>
		/// <remarks>
		/// If your game starts with a certain number of players, this callback can be useful to check the
		/// Room.playerCount and find out if you can start.
		/// </remarks>
		public virtual void OnPlayerEnteredRoom(Player newPlayer)
		{
		}

		/// <summary>
		/// Called when a remote player left the room or became inactive. Check otherPlayer.IsInactive.
		/// </summary>
		/// <remarks>
		/// If another player leaves the room or if the server detects a lost connection, this callback will
		/// be used to notify your game logic.
		///
		/// Depending on the room's setup, players may become inactive, which means they may return and retake
		/// their spot in the room. In such cases, the Player stays in the Room.Players dictionary.
		///
		/// If the player is not just inactive, it gets removed from the Room.Players dictionary, before
		/// the callback is called.
		/// </remarks>
		public virtual void OnPlayerLeftRoom(Player otherPlayer)
		{
		}

		/// <summary>
		/// Called when a previous OpJoinRandom call failed on the server.
		/// </summary>
		/// <remarks>
		/// The most common causes are that a room is full or does not exist (due to someone else being faster or closing the room).
		///
		/// When using multiple lobbies (via OpJoinLobby or a TypedLobby parameter), another lobby might have more/fitting rooms.<br/>
		/// </remarks>
		/// <param name="returnCode">Operation ReturnCode from the server.</param>
		/// <param name="message">Debug message for the error.</param>
		public virtual void OnJoinRandomFailed(short returnCode, string message)
		{
		}

		/// <summary>
		/// Called when the client is connected to the Master Server and ready for matchmaking and other tasks.
		/// </summary>
		/// <remarks>
		/// The list of available rooms won't become available unless you join a lobby via LoadBalancingClient.OpJoinLobby.
		/// You can join rooms and create them even without being in a lobby. The default lobby is used in that case.
		/// </remarks>
		public virtual void OnConnectedToMaster()
		{
		}

		/// <summary>
		/// Called when a room's custom properties changed. The propertiesThatChanged contains all that was set via Room.SetCustomProperties.
		/// </summary>
		/// <remarks>
		/// Since v1.25 this method has one parameter: Hashtable propertiesThatChanged.<br/>
		/// Changing properties must be done by Room.SetCustomProperties, which causes this callback locally, too.
		/// </remarks>
		/// <param name="propertiesThatChanged"></param>
		public virtual void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
		{
		}

		/// <summary>
		/// Called when custom player-properties are changed. Player and the changed properties are passed as object[].
		/// </summary>
		/// <remarks>
		/// Changing properties must be done by Player.SetCustomProperties, which causes this callback locally, too.
		/// </remarks>
		///
		/// <param name="targetPlayer">Contains Player that changed.</param>
		/// <param name="changedProps">Contains the properties that changed.</param>
		public virtual void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
		{
		}

		/// <summary>
		/// Called when the server sent the response to a FindFriends request.
		/// </summary>
		/// <remarks>
		/// After calling OpFindFriends, the Master Server will cache the friend list and send updates to the friend
		/// list. The friends includes the name, userId, online state and the room (if any) for each requested user/friend.
		///
		/// Use the friendList to update your UI and store it, if the UI should highlight changes.
		/// </remarks>
		public virtual void OnFriendListUpdate(List<FriendInfo> friendList)
		{
		}

		/// <summary>
		/// Called when your Custom Authentication service responds with additional data.
		/// </summary>
		/// <remarks>
		/// Custom Authentication services can include some custom data in their response.
		/// When present, that data is made available in this callback as Dictionary.
		/// While the keys of your data have to be strings, the values can be either string or a number (in Json).
		/// You need to make extra sure, that the value type is the one you expect. Numbers become (currently) int64.
		///
		/// Example: void OnCustomAuthenticationResponse(Dictionary&lt;string, object&gt; data) { ... }
		/// </remarks>
		/// <see cref="https://doc.photonengine.com/en-us/realtime/current/reference/custom-authentication"/>
		public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
		}

		/// <summary>
		/// Called when the custom authentication failed. Followed by disconnect!
		/// </summary>
		/// <remarks>
		/// Custom Authentication can fail due to user-input, bad tokens/secrets.
		/// If authentication is successful, this method is not called. Implement OnJoinedLobby() or OnConnectedToMaster() (as usual).
		///
		/// During development of a game, it might also fail due to wrong configuration on the server side.
		/// In those cases, logging the debugMessage is very important.
		///
		/// Unless you setup a custom authentication service for your app (in the [Dashboard](https://dashboard.photonengine.com)),
		/// this won't be called!
		/// </remarks>
		/// <param name="debugMessage">Contains a debug message why authentication failed. This has to be fixed during development.</param>
		public virtual void OnCustomAuthenticationFailed(string debugMessage)
		{
		}

		//TODO: Check if this needs to be implemented
		// in: IOptionalInfoCallbacks
		public virtual void OnWebRpcResponse(OperationResponse response)
		{
		}

		//TODO: Check if this needs to be implemented
		// in: IOptionalInfoCallbacks
		public virtual void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
		{
		}
		#endregion


		#region CHỨC NĂNG VÀ EVENT CHÍNH
		public override int turn { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

		public override float elapsedTurnTime => throw new NotImplementedException();

		public override float remainTurnTime => throw new NotImplementedException();

		public override bool isTurnTimeOver => throw new NotImplementedException();

		public override float ElapsePlayerTime(P player)
		{
			throw new NotImplementedException();
		}

		public override bool IsPlayerTimeOver(P player)
		{
			throw new NotImplementedException();
		}

		public override void Play(params Vector3Int[] data)
		{
			throw new NotImplementedException();
		}

		public override void QuitTurn()
		{
			throw new NotImplementedException();
		}

		public override float RemainPlayerTime(P player)
		{
			throw new NotImplementedException();
		}

		public override void Report(ReportEvent action, params object[] data)
		{
			throw new NotImplementedException();
		}

		public override void Request(RequestEvent ev)
		{
			throw new NotImplementedException();
		}

		public override void Surrender()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}