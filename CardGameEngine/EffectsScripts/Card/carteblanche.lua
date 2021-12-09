max_level = 1   
image_id = 517


name = "Carte Blanche"
pa_cost = 2

targets = {
    CreateTarget("Prendre l'effet d'une carte de la main", TargetTypes.Card, false, card_filter),
}

function card_filter(a_card)
    return EffectOwner.Hand.Contains(a_card) 
           and a_card ~= ThisCard
end


-- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return TargetsExists({1})
end 

carte_copie = nil

function description()
    if(carte_copie==nil)
    then
    
       return"Cette carte peut copier l'effet d'une carte"
    end
    return "Cette carte prend l'effet de la carte".carte_copie.Name
end

function do_effect() 
    if(carte_copie==nil) then
        carte_copie= AskForTarget(1)
        ThisCard.Cost = carte_copie.Cost
    else
        Game.PlayCard(EffectOwner,carte_copie)
    end
    
end

