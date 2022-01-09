max_level = 10 --TODO-- A définir
image_id = 519


name = "Victoire"
pa_cost = 1

-- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return true
end

function description()
    if(current_level<3)then
        return "Ameliore la carte jusqu'a"..max_level.."pour gagnez"
    elseif(max_level-current_level<5) then
        return "Plus que ".. max_level-current_level .."évolution de la carte pour terminer la partie"
    else
        return "La carte est au niveau : "..current_level
    end
end

function do_effect()
    
end

function on_level_change(oldLevel, newLevel)
    -- fonction appelée quand la carte change de niveau (OPTIONNEL)
end