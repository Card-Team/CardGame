max_level = 1
image_id = 514

name = "Échange"
pa_cost = 2

description = "Echange cette carte avec une carte aléatoire de la main de votre adversaire."

function card_filter()
    -- Verifier quand pas de carte
    local OtherHand = EffectOwner.OtherPlayer.Hand
    local random = math.random(0, OtherHand.Count- 1)
    return OtherHand[random]
end

targets = {
    CreateTarget("La carte que l'on veut échanger", TargetTypes.Card, true, card_filter),
}


-- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return EffectOwner.OtherPlayer.Hand.Count > 0
end

function do_effect()
    local theCard = AskForTarget(1)
    -- TODO échanger leur position aussi ( donc pas 0)
    EffectOwner.OtherPlayer.Hand.MoveTo(EffectOwner.Hand, theCard, 0)
    EffectOwner.Hand.MoveTo(EffectOwner.OtherPlayer.Hand, This, 0)
end

