max_level = 1
image_id = 626


name = "Espion"
pa_cost = 3
targets = {
    CreateTarget("carte qui fait voir", TargetTypes.Card, true, cardFilter),
}
--fonction qui recupere toute les cartes de sa main
function cardFilter()
    -- carte choisis aleatoirement depuis sa main
    local OtherPlayerHand = EffectOwner.OtherPlayer.Hand
    local random = math.random(0, OtherPlayerHand.Count() - 1)
    return OtherPlayerHand[random]
end

function precondition()
    return TargetsExists({ 1 })
end

function description()
    return "voir une carte de la main de ton adversaire"
end

function do_effect()
    --on recupere la carte de l'adversaire
    local card = AskForTarget(1)                        --la carte
    EffectOwner.OtherPlayer.RevealCard(card)            -- fait voir la carte selectionner de la main de l'adversaire
end