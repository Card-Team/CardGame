max_level = 1
image_id = 626

name = "Espion"
pa_cost = 3

description = "voir une carte aleatoire de la main de l'adversaire"

--fonction qui recupere toute les cartes de sa main
function cardFilter()
    -- carte choisis aleatoirement depuis sa main
    local OtherPlayerHand = EffectOwner.OtherPlayer.Hand
    local random = math.random(0, OtherPlayerHand.Count - 1)
    return OtherPlayerHand[random]
end

targets = {
    CreateTarget("carte qui fait voir", TargetTypes.Card, true, cardFilter),
}


function precondition()
    return EffectOwner.OtherPlayer.Hand.Count > 0
end

function do_effect()
    --on recupere la carte de l'adversaire
    local card = AskForTarget(1)                        --la carte
    Game.RevealCard(EffectOwner,card)                   -- fait voir la carte selectionner de la main de l'adversaire
end