# Documentation Lua

Vous retrouverez ici la documentation nécessaire pour faire de scripts d'effets en lua.

# Pour n'importe quel objet

## Éléments c# accessibles

| Nom             | [Card](CardGameEngine/Cards/Card.cs) | [Artefact](CardGameEngine/Cards/Artefact.cs) | [Keyword](CardGameEngine/Cards/Keyword.cs) |
|-----------------|:------------------------------------:|:--------------------------------------------:|:------------------------------------------:|
| AskForTarget()  |                  ✔                   |                      ✔                       |                     ❌                      |
| CreateTarget()  |                  ✔                   |                      ✔                       |                     ❌                      |
| TargetsExist()  |                  ✔                   |                      ✔                       |                     ❌                      |
| SubscribeTo()   |                  ✔                   |                      ✔                       |                     ✔                      |
| UnsubscribeTo() |                  ✔                   |                      ✔                       |                     ✔                      |
|                 |                                      |                                              |                                            |
| EffectOwner     |                  ✔                   |                      ✔                       |                     ✔                      |
| Event*          |                  ✔                   |                      ✔                       |                     ✔                      |
| Game            |                  ✔                   |                      ✔                       |                     ✔                      |
| TargetTypes     |                  ✔                   |                      ✔                       |                     ✔                      |
| This            |                  ✔                   |                      ✔                       |                     ✔                      |
| ThisCard        |                  ❌                   |                      ❌                       |                     ✔                      |

**Tous les events sont accessibles par leur nom*

## Explications

### Fonctions

`AskForTarget(int i): Card/Player` → Permet d'avoir la sélection du joueur pour la cible i du tableau ``targets``

`CreateTarget(string nomDeLaCible, TargetTypes type, bool choixCibleManuel, fonction card_filter)`: Target → Crée
une [Target](CardGameEngine/GameSystems/Targeting/Target.cs) selon les paramètres entrés.

`TargetsExist(int[]): bool` → Teste dans le tableau `targets` s'il y a au moins un des éléments pouvant être ciblé

`SubscribeTo(Event event, fonction écouteur, bool écouterMêmeAnnulé, bool écouterQueAprèsExecution)` → Affecte une
fonction lua comme écouteur de l'évènement donné

`UnsubscribeTo(Event event, fonction écouteur, bool seDésabonnerd'AprèsExecution)` → Désabonne une fonction de l'écoute
d'un évènement

[comment]: <> (TODO mettre les exemples)

### Variables

| Nom           | Type                                                                  |
|---------------|:----------------------------------------------------------------------|
| EffectOwner   | [EventProperty\<int>](CardGameEngine/GameSystems/Player.cs)           |
| Game          | [EventProperty\<int>](CardGameEngine/Game.cs)                         |
| This*         | Card/Artefact/Keyword                                                 |
| ThisCard      | [EventProperty\<int>](CardGameEngine/Cards/Card.cs)                   |
**this prend la valeur de l'object actuel, dans un script de carte, il vaudra la carte elle meme, dans une script d'artéfact, l'artéfact lui meme, etc.

# Cartes

Cette section est consacrée aux effets de cartes.

## Éléments requis

Voir [le fichier d'exemple](CardGameEngine/EffectsScripts/Card/example.lua)

Un script de carte a besoin de 4 propriétés globales qui doivent etre déclarés en haut du fichier

Propriétés immutables (qui ne changeront jamais)

`max_level` qui est le niveau maximum de la carte (les niveaux commencent a 1)

`image_id` qui sera le numéro de l'image associée a la carte

Propriétés mutables (qui pourront changer)

`name` Le nom affiché de la carte

`pa_cost` Le cout en PA de la carte

Il faut ensuite un tableau de cibles, voir [Ciblage](#ciblage)

```lua
targets = {
-- Nom, Type, Automatique ou non,Fonction de filtre des cibles potentielles
CreateTarget("Une cible carte", TargetTypes.Card, false, card_filter),
CreateTarget("Un joueur", TargetTypes.Player, true),
}
```

Les cartes demandent ensuite plusieurs fonctions qui doivent etre déclarés

* Une fonction `precondition`
Cette fonction doit inspecter l'état de la partie et déterminer si la carte a le droit d'etre jouée ou non

Elle renvoit un boolen, si il vaut `true` le joueur aura l'opportunité de jouer la carte
si il vaut `false` le joueur ne pourra pas jouer la carte

Cette méthode sera déclenchée souvent

* Une fonction `description`
Cette fonction doit renvoyer le texte de description de la carte sous forme de chaine de caracteres

Elle peut etre dynamique, mais pour l'instant elle ne sera appelée que apres un changement de niveau

* Une fonction `do_effect`
Cette fonction est censé réaliser l'effet de la carte



Il y a aussi deux méthodes optionelles

* `on_level_change(oldlevel,newlevel)`
Elle est appelée lorsque la carte change de niveau

* `on_card_create()`
Elle est appelé au moment ou la carte est "créée", donc soit au début de la partie pour les cartes du deck,
soit au moment de la création pour les cartes fabriqués pendant la partie


## Nom de fichier

Le nom du fichier d'un script de carte doit etre le nom de la carte en snake_case (minuscules avec underscore a la place des espaces)

Si une carte est "virtuelle" (elle ne peut pas etre dans un deck et doit forcément etre crée durant la partie)
son nom commence par une underscore



# Artefact
todo

# Keyword
todo

# Systemes

## Ciblage

Le systeme de ciblage fonctionne en deux temps

D'abord la création des parametres de ciblage, ensuite son utilisation

La création des parametre se fait avec la fonctio `CreateTarget`

Le nom et le type d'une cible sont assez explicites, il ne sont donc pas detaillés ici

Le booleen `automatic` change de maniere important la résolution d'une cible

Au moment ou une cible doit etre résolue, si `automatic` vaut false :

1. **tous** les "elements" en jeu sont passés par la fonction de filtre de la cible, si elle en a une
2. La fonction de filtre renvoit `true` ou `false` pour chacune des cibles
3. Le moteur du jeu garde uniquement celles qui ont renvoyé `true`
4. Le moteur du jeu demande au joueur de choisir parmi les cibles gardés
5. Le joueur choisit
6. Le moteur du jeu donne la cible choisi a l'effet qui en avait demandé la résolution

Par contre, si `automatic` vaut vrai, l'effet lui meme doit donner une cible comme ceci :

1. Le moteur du jeu appelle la fonction de filtre de cible **sans arguments**, il est aussi **obligatoire** d'avoir une fonction de filtre ici
2. La fonction de cible doit **retourner une cible** et c'est celle ci qui va etre renvoyé a l'effet qui a démandé la résolution de la cible

Comme vous pouvez le voir, dans le cas d'une cible automatique, la fonction de filtre n'est en fait plus une fonction de filtre du tout,

mais une fonction qui renvoit la cible.