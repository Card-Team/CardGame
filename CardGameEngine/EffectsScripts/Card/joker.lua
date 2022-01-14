---@module joker
max_level = 2
image_id = 520

name = "Joker"
pa_cost = 2

base_description = "Cette carte peut copier l'effet d'une carte du deck"
description = base_description

function card_filter(a_card)
    local cardDeckPlayer = EffectOwner.Deck
    local random = math.random(0, cardDeckPlayer.Count - 1)
    return cardDeckPlayer[random]
end

targets = {
    CreateTarget("Effet d'une carte aléatoire de ton deck", TargetTypes.Card, true, card_filter),
    CreateTarget("Effet de 2 cartes aléatoires de ton deck", TargetTypes.Card, true, card_filter)
}


--Joker : lv1 : Carte copie l'effet d'une carte aléatoirement contenue dans son deck.
--        lv2 : le joueur applique deux effets parmi les cartes de son deck aléatoirement et simultanément.
function precondition()
    return EffectOwner.Deck.Count > 0
end

function do_effect()
    if (current_level == max_level) then
        --application des 2 effets cartes lvl2
        effet1 = AskForTarget(1).Virtual()                   --var qui recuper l'effet 1
        effet2 = AskForTarget(2).Virtual()                   --var qui recuper l'effet 2
        Game.PlayCardVirtual(EffectOwner, effet1)                   --joue l'effet de la 1 carte
        Game.PlayCardVirtual(EffectOwner, effet2)                   --joue l'effet de la 2 carte
    else
        effet1 = AskForTarget(1).Virtual()
        Game.PlayCardVirtual(EffectOwner, effet1)
    end
end

function on_level_change(old, new)
    if (new == max_level) then
        This.Description.TryChangeValue("Cette carte peut copier l'effet d'une carte du deck")
    else
        This.Description.TryChangeValue(base_description)
    end
end
