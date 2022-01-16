

---@class Game
---@field public CurrentPlayer Player
---@field public Player1 Player
---@field public Player2 Player
---@field public MakeWin fun(playerToWin:Player)
---@field public TryEndPlayerTurn fun():boolean
---@field public PlayCard fun(effectowner:Player,card:Card):boolean
---@field public PlayCard fun(effectowner:Player,card:Card,discardSource:CardPile,discardGoal:DiscardPile):boolean
---@field public PlayCardEffect fun(effectowner:Player,card:Card):boolean
---@field public GetCurrentOwner fun(card:Card):Player
---@field public GetPileOf fun(card:Card):CardPile
---@field public RevealCard fun(player:Player,card:Card)
---@field public ActivateArtifact fun(player:Player,artefact:Artefact)
---@field public ChooseBetween fun(player:Player,cards:Card[]):Card
---@field public MakeVirtual fun(nom:string,description:string):Card
---@field public MakeVirtual fun(nom:string,description:string,imageId:number | nil,effect:fun()):Card


---@class Player
---@field public Deck CardPile
---@field public Hand CardPile
---@field public Discard DiscardPile
---@field public Artefacts Artefact[]
---@field public ActionPoints EventProperty<Player,number,ActionPointsEditEvent>
---@field public MaxActionPoints EventProperty<Player,number,MaxActionPointsEditEvent>
---@field public OtherPlayer Player
---@field public Cards Card[]
---@field public DrawCard fun()
---@field public HasCard fun(card:Card):boolean
---@field public LoopDeck fun()


---@class ITargetable


---@class Target
---@field public TargetType number
---@field public IsAutomatic boolean
---@field public Name string
---@field public IsValidTarget fun(card:Card):boolean
---@field public GetAutomaticTarget fun():ITargetable


---@class TargetTypes
---@field public Player TargetTypes
---@field public Card TargetTypes


---@class EventProperty<S,T,ET>
---@field public Value T
---@field public TryChangeValue fun(newVal:T):T
---@field public ToString fun():string
---@field public StealthChange fun(value:T)


---@class ActionPointsEditEvent : CancellableEvent
---@field public Player Player
---@field public OldPointCount number
---@field public NewPointCount number


---@class CancellableEvent : Event


---@class Event


---@class MaxActionPointsEditEvent : CancellableEvent
---@field public Player Player
---@field public OldMaxPointCount number
---@field public NewMaxPointCount number


---@class DeckLoopEvent : Event
---@field public Player Player


---@class EndTurnEvent : CancellableEvent
---@field public Player Player


---@class StartTurnEvent : Event
---@field public Player Player


---@class CardDeleteEvent : TransferrableCardEvent


---@class CardEvent : CancellableEvent
---@field public Card Card


---@class CardMarkUpgradeEvent : CardEvent


---@class CardMovePileEvent : CardEvent
---@field public SourcePile CardPile
---@field public SourceIndex number
---@field public DestPile CardPile
---@field public DestIndex number


---@class CardPlayEvent : TransferrableCardEvent
---@field public WhoPlayed Player


---@class CardUnMarkUpgradeEvent : CardEvent


---@class TransferrableCardEvent : CardEvent
---@field public Card Card


---@class TargetingEvent : Event
---@field public TargetData Target
---@field public ResolvedTarget ITargetable


---@class CardCostChangeEvent : CardPropertyChangeEvent<number>


---@class CardDescriptionChangeEvent : CardPropertyChangeEvent<string>


---@class CardLevelChangeEvent : CardPropertyChangeEvent<number>


---@class CardNameChangeEvent : CardPropertyChangeEvent<string>


---@class CardPropertyChangeEvent<T> : CardEvent
---@field public OldValue T
---@field public NewValue T


---@class CardKeywordAddEvent : CardKeywordEvent


---@class CardKeywordEvent : CardEvent
---@field public Keyword Keyword


---@class CardKeywordRemoveEvent : CardKeywordEvent


---@class CardKeywordTriggerEvent : CardKeywordEvent


---@class ArtefactActivateEvent : ArtefactEvent


---@class ArtefactChargeEditEvent : ArtefactEvent
---@field public NewChargeCount number
---@field public OldChargeCount number


---@class ArtefactEvent : CancellableEvent
---@field public Artefact Artefact


---@class Artefact
---@field public Name string
---@field public MaxCharge number
---@field public CurrentCharge EventProperty<Artefact,number,ArtefactChargeEditEvent>
---@field public CanBeActivated fun(game:Game):boolean


---@class Card
---@field public Name EventProperty<Card,string,CardNameChangeEvent>
---@field public IsVirtual boolean
---@field public MaxLevel number
---@field public Cost EventProperty<Card,number,CardCostChangeEvent>
---@field public CurrentLevel EventProperty<Card,number,CardLevelChangeEvent>
---@field public Description EventProperty<Card,string,CardDescriptionChangeEvent>
---@field public Keywords Keyword[]
---@field public IsMaxLevel boolean
---@field public Virtual fun():Card
---@field public Clone fun():Card
---@field public CanBePlayed fun(effectOwner:Player):boolean
---@field public Upgrade fun():boolean
---@field public ToString fun():string
---@field public OnLevelChange fun(oldLevel:number,newLevel:number)


---@class Keyword
---@field public Name string


---@class CardPile
---@field public Count number
---@field public IsEmpty boolean
---@field public [number] Card
---@field public Contains fun(card:Card):boolean
---@field public IndexOf fun(card:Card):number
---@field public MoveTo fun(newCardPile:CardPile,card:Card,newPosition:number):boolean
---@field public MoveInternal fun(card:Card,newPosition:number):boolean
---@field public ToString fun():string


---@class DiscardPile : CardPile
---@field public MoveForUpgrade fun(oldLocation:CardPile,toUp:Card):boolean
---@field public MoveTo fun(newCardPile:CardPile,card:Card,newPosition:number):boolean
---@field public UnMarkForUpgrade fun(card:Card)
---@field public IsMarkedForUpgrade fun(card:Card):boolean


---@class IEventHandler
---@field public EvenIfCancelled boolean
---@field public PostEvent boolean
---@field public EventType Type<Event>
---@field public HandleEvent fun(evt:Event)


-- EFFETS

---@class Type<T:Event>

---@type Type<ActionPointsEditEvent>
ActionPointsEditEvent = --[[---@type Type<ActionPointsEditEvent>]] {}

---@type Type<CancellableEvent>
CancellableEvent = --[[---@type Type<CancellableEvent>]] {}

---@type Type<Event>
Event = --[[---@type Type<Event>]] {}

---@type Type<MaxActionPointsEditEvent>
MaxActionPointsEditEvent = --[[---@type Type<MaxActionPointsEditEvent>]] {}

---@type Type<DeckLoopEvent>
DeckLoopEvent = --[[---@type Type<DeckLoopEvent>]] {}

---@type Type<EndTurnEvent>
EndTurnEvent = --[[---@type Type<EndTurnEvent>]] {}

---@type Type<StartTurnEvent>
StartTurnEvent = --[[---@type Type<StartTurnEvent>]] {}

---@type Type<CardDeleteEvent>
CardDeleteEvent = --[[---@type Type<CardDeleteEvent>]] {}

---@type Type<CardEvent>
CardEvent = --[[---@type Type<CardEvent>]] {}

---@type Type<CardMarkUpgradeEvent>
CardMarkUpgradeEvent = --[[---@type Type<CardMarkUpgradeEvent>]] {}

---@type Type<CardMovePileEvent>
CardMovePileEvent = --[[---@type Type<CardMovePileEvent>]] {}

---@type Type<CardPlayEvent>
CardPlayEvent = --[[---@type Type<CardPlayEvent>]] {}

---@type Type<CardUnMarkUpgradeEvent>
CardUnMarkUpgradeEvent = --[[---@type Type<CardUnMarkUpgradeEvent>]] {}

---@type Type<TransferrableCardEvent>
TransferrableCardEvent = --[[---@type Type<TransferrableCardEvent>]] {}

---@type Type<TargetingEvent>
TargetingEvent = --[[---@type Type<TargetingEvent>]] {}

---@type Type<CardCostChangeEvent>
CardCostChangeEvent = --[[---@type Type<CardCostChangeEvent>]] {}

---@type Type<CardDescriptionChangeEvent>
CardDescriptionChangeEvent = --[[---@type Type<CardDescriptionChangeEvent>]] {}

---@type Type<CardLevelChangeEvent>
CardLevelChangeEvent = --[[---@type Type<CardLevelChangeEvent>]] {}

---@type Type<CardNameChangeEvent>
CardNameChangeEvent = --[[---@type Type<CardNameChangeEvent>]] {}

---@type Type<CardPropertyChangeEvent<any>>
CardPropertyChangeEvent = --[[---@type Type<CardPropertyChangeEvent<any>>]] {}

---@type Type<CardKeywordAddEvent>
CardKeywordAddEvent = --[[---@type Type<CardKeywordAddEvent>]] {}

---@type Type<CardKeywordEvent>
CardKeywordEvent = --[[---@type Type<CardKeywordEvent>]] {}

---@type Type<CardKeywordRemoveEvent>
CardKeywordRemoveEvent = --[[---@type Type<CardKeywordRemoveEvent>]] {}

---@type Type<CardKeywordTriggerEvent>
CardKeywordTriggerEvent = --[[---@type Type<CardKeywordTriggerEvent>]] {}

---@type Type<ArtefactActivateEvent>
ArtefactActivateEvent = --[[---@type Type<ArtefactActivateEvent>]] {}

---@type Type<ArtefactChargeEditEvent>
ArtefactChargeEditEvent = --[[---@type Type<ArtefactChargeEditEvent>]] {}

---@type Type<ArtefactEvent>
ArtefactEvent = --[[---@type Type<ArtefactEvent>]] {}

