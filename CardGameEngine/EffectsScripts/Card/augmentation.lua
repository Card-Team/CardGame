max_level = 4
image_id = 523

name = "Augmentation"
pa_cost = 2

description = "(bloqué avant level 4) Augmentation du nombre de PA max de 1"

function card_filter()
    -- carte choisis aleatoirement depuis ton deck
    return EffectOwner.Player
end

targets = {
    CreateTarget("le joueur dont son nombre de PA va diminuer", TargetTypes.Player, true, card_filter),
}


--- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
        return This.CurrentLevel.Value > max_level - 1 
end

function do_effect()
    local Player = AskForTarget(1)                                        --cout de la carte 
    local max_Action_Point = Player.MaxActionPoints.Value                --PA max du joueur
    max_Action_Point.TryChangeValue(max_Action_Point + 1)        --on enleve au cout d'action le cout de la carte
end

function on_level_change(old,new)
    if new == 4 then
        This.Description.TryChangeValue("Augmentation du nombre de PA max de 1")
    end
end 