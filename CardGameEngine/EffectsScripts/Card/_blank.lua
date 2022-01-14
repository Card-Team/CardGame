---@module _blank
max_level = 1
image_id = 519

name = "Vide"
pa_cost = 1

description = "vide"

targets = {}

-- carte vide par défaut
-- elle est chargée automatiquement quand une carte est virtuelle
function precondition()
    return true
end

function do_effect()

end

