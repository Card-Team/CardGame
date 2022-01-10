max_level = 1
image_id = 523

name = "affaiblissement"
pa_cost = 3

targets = {
    CreateTarget("points d'action baisser aleatoirement", TargetTypes.Card, true, card_filter),
}

function card_filter()
    -- carte choisis aleatoirement depuis ton deck
    local CardPile = EffectOwner.Player.Deck
    local random = math.random(0, CardPile.Count() - 1)
    return CardPile[random]
end

--- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return TargetsExists({ 1 })
end

function description()
    return "faire baisser le cout d'action de l'adversaire avec le cout d'une carte aleatoire"
end

function do_effect()
    --prends le cout de CardPile est enleve le nombre de pints d'action a l'edversaire
    local coutCard = AskForTarget(1).Cost.Value                                         --cout de la carte
    local pointActionAdv = EffectOwner.OtherPlayer.ActionPoints.Value                   --point d'action de l'adversaire
    EffectOwner.OtherPlayer.ActionPoints.TryChangeValue(pointActionAdv - coutCard)        --on enleve au cout d'action le cout de la carte
end