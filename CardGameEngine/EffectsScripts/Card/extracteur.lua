max_level = 3
image_id = 613


name = "Extracteur"
pa_cost = 3
--TODO a definir le nombre de cout
targets = {
    CreateTarget("Carte a gagner ses pointchrome://resources/css/ction", TargetTypes.Card, false, cardFilter),
}
--fonction qui recupere toute les cartes de sa main sauf celle ci est lui demande d'en selectionné une
function cardFilter(aCard)
    return EffectOwner.Hand.Contains(aCard) 
    and ThisCard ~= aCard
end

--Extracteur  : Carte qui permet au joueur de choisir parmi une de ses cartes en main et d'en gagner le cout en points d'action.
--La carte choisit retourne en bas du deck et devient lourde jusqu à prochaine pioche. Lv1 -> Cout max de la carte : 2, Lv2-> Cout max de la carte : 3, lv3 -> Cout max de la carte : 4 "

function precondition()
    return TargetsExists({ 1 })
end 

function description()
    return "Gagner les points d'action d'une carte de ton deck selectionné"
end

function do_effect()
    --on recupere les points d'action de la carte choisis et on modifie grace a TryChangeValue son cout actuelle
    local coutCart = aCard.Cost();
    EffectOwner.ActionPoints.TryChangeValue(coutCart);
end