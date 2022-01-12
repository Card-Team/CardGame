max_level = 1
image_id = 523

name = "Amélioration Débug"
pa_cost = 0

description = "Augement le niveau d'une carte de 5"

function card_filter(a_card)
    return not a_card.IsMaxLevel
end

targets = {
    CreateTarget("Carta a augmenter", TargetTypes.Card, false, card_filter),
}


--- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return TargetsExists({1})
end

function do_effect()
    local toUp = AskForTarget(1)
    local newLevel = math.min(toUp.CurrentLevel.Value + 5,toUp.MaxLevel)
    if(newLevel ~= toUp.CurrentLevel.Value) then
        toUp.CurrentLevel.TryChangeValue(newLevel)
    end
    return false
end