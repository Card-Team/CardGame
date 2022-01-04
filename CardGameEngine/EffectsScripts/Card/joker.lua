max_level = 1 
image_id = 520


name = "Joker"
pa_cost = 2

targets = {
    CreateTarget("la carte en laquelle se transformer", TargetTypes.Card, false, card_filter),
}

function card_filter(a_card)
    return EffectOwner.Hand.Contains(a_card)
            and a_card ~= ThisCard
end


--Joker : Carte copie l'effet d'une carte aléatoirement contenue dans le deck et en appliquer l'effet lvl 1.
--lvl 2 le joueur applique deux effets parmi les cartes de son deck aléatoirement et simultanément.
function precondition()
    return TargetsExists({1})
end

carte_copie = nil

function description()
    if(carte_copie==nil)
    then
        if(current_level == max_level)then
            return"Cette carte peut copier l'effet de plusieurs cartes du deck"
        end
        else
            return"Cette carte peut copier l'effet d'une carte dans le deck".carte_copie.Name
    end
    return 
end
function description()

end


function do_effect()
    if(current_level ~= max_level)then
        
    end
    if(carte_copie==nil) then
        carte_copie= AskForTarget(1)
        ThisCard.Cost = carte_copie.Cost
    else
        Game.PlayCard(EffectOwner,carte_copie)
    end
    
end  
