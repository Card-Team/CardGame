max_level = 2
image_id = 500

name = "Nom"
pa_cost = 2

description = "une description de la carte qui peut changer"

targets = {
    -- Nom, Type, Automatique ou non,Fonction de filtre des cibles potentielles
    CreateTarget("Une cible carte", TargetTypes.Card, false, card_filter),
    CreateTarget("Un joueur", TargetTypes.Player, true),
}

function card_filter(a_card)
    -- permet uniquement le ciblage de carte ayant comme nom 'Exemple'
    return a_card.Name == "Exemple"
end

-- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    -- la carte peut être jouée sans aucun critère spécifiques
    return true
end

function do_effect()
    -- le code de l'effet de la carte
end

function on_level_change(oldLevel, newLevel)
    -- fonction appelée quand la carte change de niveau (OPTIONNEL)
end