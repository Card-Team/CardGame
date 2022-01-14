---@module debugamelioration
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

    local up = Game.MakeVirtual("Augmentation", "augmentation 5")
    local down = Game.MakeVirtual("Diminution", "diminution 5")

    local chosen = Game.ChooseBetween(EffectOwner, up, down)

    Game.PlayCardEffect(EffectOwner, chosen)

    local toUp = AskForTarget(1)
    local newLevel = toUp.CurrentLevel.Value
    if chosen == up then
        newLevel = math.min(newLevel + 5, toUp.MaxLevel)
    else
        newLevel = math.max(newLevel - 5, 0)
    end
    if (newLevel ~= toUp.CurrentLevel.Value) then
        toUp.CurrentLevel.TryChangeValue(newLevel)
    end
    return false
end