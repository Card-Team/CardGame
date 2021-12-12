# Documentation Lua

Vous retrouverez ici la documentation nécessaire pour faire de scripts d'effets en lua.

# Pour n'importe quel objet

## Éléments c# accessibles

|Nom             |[Card](CardGameEngine/Cards/Card.cs)|[Artefact](CardGameEngine/Cards/Artefact.cs)|[Keyword](CardGameEngine/Cards/Keyword.cs)|
|---------------|:----------:|:----------:|:----------:|
|AskForTarget() |      ✔     |      ✔     |     ❌     |
|CreateTarget() |      ✔     |      ✔     |     ❌     |
|TargetsExist() |      ✔     |      ✔     |     ❌     |
|SubscribeTo()  |      ✔     |      ✔     |     ✔     |
|UnsubscribeTo()|      ✔     |      ✔     |     ✔     |
||||
|CurrentCharge  |      ❌     |      ✔     |      ❌    |
|CurrentLevel   |      ✔     |      ❌     |     ❌     |
|CurrentName    |      ✔     |      ✔     |      ✔     |
|CurrentPACost  |      ✔     |      ❌     |     ❌     |
|EffectOwner    |      ✔     |      ✔     |      ✔     |
|Event*         |      ✔     |      ✔     |      ✔     |
|Game           |      ✔     |      ✔     |      ✔     |
|TargetTypes    |      ✔     |      ✔     |      ✔     |
|This           |      ✔     |      ✔     |      ✔     |
|ThisCard       |      ❌     |     ❌     |      ✔     |

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

|Nom             |Type      |
|---------------|-----------|
|CurrentCharge  |EventProperty\<int>|
|CurrentLevel   |EventProperty\<int>|
|CurrentName    |EventProperty\<string>|
|CurrentPACost  |EventProperty\<int>|
|EffectOwner    |Player|
|Game           |Game|
|This           |Card/Artefact/Keyword|
|ThisCard       |Card|

# Cartes

Cette section est consacrée aux effets de cartes.

## Éléments requis
