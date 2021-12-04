max_level = 2
image_id = 500


name = "Nom"
pa_cost = 2

targets = {
    -- Nom, Type, Automatique ou non,Fonction de filtre des cibles potentielles
    CreateTarget("Une cible carte", TargetTypes.Card, false, cardFilter),
    CreateTarget("Un joueur", TargetTypes.Player, true),
}

function cardFilter(aCard)
    -- permet uniquement le ciblage de carte ayant comme nom 'Exemple'
    return aCard.Name == "Exemple"
end 

-- fonction qui renvoit un booleen si la carte peut etre jouée ou non
function precondition()
    -- la carte peut etre jouée sans aucun critere spécifiques
    return true
end 

function description()
    return "une description de la carte qui peut changer"
end

function do_effect()
    -- le code de l'effet de la carte
end