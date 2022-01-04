max_level = 1
image_id = 549


name = "Carte Jocker"
pa_cost = x--Cout moyen en point d'action du deck

targets = {
    CreateTarget("Prend l'effet d'une carte aléatoire du deck", TargetTypes.Card, false, card_filter),
}

function card_filter(a_card)

end


function precondition()
    return TargetsExists({ 1 })
end


function description()
    return "Prend l'effet d'une carte aléatoire du deck. Prix d'execution = Cout moyen en point d'action du deck"
end

function do_effect()
    
end

