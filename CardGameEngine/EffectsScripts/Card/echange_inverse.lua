max_level = 1
image_id = 518

name = "Echange inversé"
pa_cost = 2

description = "Echange cette carte avec une carte aléatoire dans votre défausse."

function card_filter()
    -- Verifier quand pas de carte dans la défausse
    local DiscardPile = EffectOwner.Discard
    local random = math.random(0, DiscardPile.Count() - 1)
    return DiscardPile[random]
end

targets = {
    CreateTarget("la carte avec laquelle elle s'échange dans la défausse", TargetTypes.Card, true, card_filter),
}


--- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return EffectOwner.Discard.Count > 0
end

function do_effect()
    local theCard = AskForTarget(1)
    -- TODO échanger leur position aussi ( donc pas 0)
    EffectOwner.Player.Discard.MoveTo(EffectOwner.Hand, theCard, 0)
    EffectOwner.Hand.MoveTo(EffectOwner.Player.Discard, This, 0)
end


