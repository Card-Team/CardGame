--immutable data
max_level = 3
image_id = 512

--mutable data
name = "Montgolfière"
pa_cost = 2

base_description = "Remonte une carte de 1 place dans le deck."
description = base_description

targets = {
    CreateTarget("La carte à remonter", TargetTypes.Card, false, card_filter)
}

function card_filter(a_card)
    return EffectOwner.Deck.Contains(a_card)
            and
            (
                    current_level == max_level
                            or
                            EffectOwner.Deck.IndexOf(a_card) > 0
            )
end

function precondition()
    return TargetsExists({ 1 })
end

function do_effect()
    local theCard = AskForTarget(1)

    local currentPos = EffectOwner.Deck.IndexOf(theCard)

    local newPos = currentPos - nb_to_move[current_level]
    if (newPos < 0 and current_level == max_level) then
        EffectOwner.DrawCard(theCard)
    else
        EffectOwner.Deck.MoveInternal(theCard, math.max(0, newPos))
    end
end

nb_to_move = { 1, 2, 3 }
function on_level_change(old, new)
    if (new == 1) then
        This.Description.TryChangeValue(base_description)
    else
        desc = "Remonte une carte de " .. nb_to_move[new] .. " places dans le deck."
        if (new == max_level) then
            desc = desc .. "\nSi la carte doit dépasser le haut du deck, vous la piochez"
        end
        This.Description.TryChangeValue(desc)
    end
end 