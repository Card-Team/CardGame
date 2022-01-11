max_level = 1
image_id = 517

name = "Carte Blanche"
pa_cost = 2

base_description = "Cette carte peut copier l'effet d'une carte"
description = base_description

function card_filter(a_card)
    return EffectOwner.Hand.Contains(a_card)
            and a_card ~= ThisCard
end

targets = {
    CreateTarget("Prendre l'effet d'une carte de la main", TargetTypes.Card, false, card_filter),
}



-- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return TargetsExists({ 1 })
end

carte_copie = nil
function do_effect()
    if (carte_copie == nil) then
        carte_copie = AskForTarget(1)
        This.Cost = carte_copie.Cost
        This.Description.TryChangeValue("Cette carte prend l'effet de la carte " .. carte_copie.Name)
    else
        Game.PlayCard(EffectOwner, carte_copie)
        This.Description.TryChangeValue(base_description)
    end

end

