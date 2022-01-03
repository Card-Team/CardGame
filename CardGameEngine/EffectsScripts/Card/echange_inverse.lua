max_level = 1   
image_id = 518


name = "Echange inversé"
pa_cost = 2

targets = {
    CreateTarget("Echange inverse", TargetTypes.Card, false, card_filter),
}

function card_filter(a_card)
    -- Verifier quand pas de carte dans la défausse
    local DiscardPile = EffectOwner.Player.Discard
    math.randomseed(os.time())
    local random = math.random(0, DiscardPile.Count() - 1)
    return DiscardPile[random]
end


--- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return TargetsExists({ 1 })
end

function description()
    return "Echange cette carte avec une carte aléatoire dans votre défausse."
end

function do_effect()
    local theCard = AskForTarget(1)
    -- TODO échanger leur position aussi ( donc pas 0)
    EffectOwner.Player.Discard.MoveTo(EffectOwner.Hand, theCard, 0)
    EffectOwner.Hand.MoveTo(EffectOwner.Player.Discard, ThisCard, 0)
end


