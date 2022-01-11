max_level = 1
image_id = 709

name = "chance"
pa_cost = 2

description = "Devine une carte de la main du joueur si c'est la bonne elle va dans sa defausse"

function cardFilter()
    local OtherHand = EffectOwner.OtherPlayer.Hand
    --selectionner toute les cartes du jeu
    local card = EffectOwner.OtherPlayer.Discard+EffectOwner.OtherPlayer.Deck+EffectOwner.OtherPlayer.Hand
    --contains pour verifier si il a selectionner une carte de sa main
    if OtherHand.Contains(card) then
        --faire le do effect
    else 
        print("Dommage c'est pas la bonne")
    end
end

targets = {
    --cible avec la precondition qui renvoie faux s'il y'a aucune carte dans sa main
    CreateTarget("Devine une carte de la main du joueurs", TargetTypes.Card, true, cardFilter),
}


function precondition()
    return EffectOwner.OtherPlayer.Hand.Count > 0
end

function do_effect()
    --TODO
    local carte = AskForTarget(1)                                                      --carte
    EffectOwner.OtherPlayer.Hand.MoveTo(EffectOwner.OtherPlayer.Discard, carte, 0)     --prends la carte de l'adversaire depuis la main et la met dans sa defausse
end