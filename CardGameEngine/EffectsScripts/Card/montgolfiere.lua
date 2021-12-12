--immutable data
max_level = 3
image_id = 512

--mutable data
name = "Montgolfière"
pa_cost = 2
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

nb_to_move = { 1, 2, 3 }

function precondition()
    return TargetsExists({ 1 })
end

function description()
    local nb = nb_to_move[current_level]
    local hasS = nb > 1 and "s" or ""
    local desc = "Remonte une carte de " .. nb .. " position" .. hasS .. " dans le deck."
    if (current_level == max_level) then
        desc = desc .. "\n"
        desc = desc .. "Si la carte doit dépasser le haut du deck, vous la piochez."
    end
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