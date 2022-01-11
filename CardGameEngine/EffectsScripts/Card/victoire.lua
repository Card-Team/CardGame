max_level = 10 --TODO-- A définir
image_id = 519

name = "Victoire"
pa_cost = 1

base_description = "Améliore la carte jusqu'au niveau " .. max_level .. " pour gagner."
description = base_description

targets = {}

-- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return false
end

function do_effect()

end

function on_level_change(old, new)
    if (new < 3) then
        This.Description.TryChangeValue(base_description)
    elseif (max_level - new < 5) then
        This.Description.TryChangeValue("Plus que " .. max_level - current_level .. " améliorations de la carte pour gagner la partie.")
    else
        This.Description.TryChangeValue("La carte est au niveau : " .. current_level)
    end
end