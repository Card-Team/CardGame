max_level = 1
image_id = 517

name = "Carte Blanche"
pa_cost = 2

base_description = "Cette carte jouer l'effet d'une carte de ta main"
description = base_description

function card_filter(a_card)
    return EffectOwner.Hand.Contains(a_card)
            and a_card.Name.Value ~= This.Name.Value
end

targets = {
    CreateTarget("jouer un effet d'une carte de ta main", TargetTypes.Card, false, card_filter),
}



-- fonction qui renvoie un booléen si la carte peut être jouée ou non
function precondition()
    return TargetsExists({ 1 })
end

carte_copie = nil
function do_effect()
    if (carte_copie == nil) then
        carte_copie = AskForTarget(1).Virtual()         --creer une copie virtuel de la carte ciblé
        This.Cost.TryChangeValue(carte_copie.Cost.Value)
        This.Description.TryChangeValue("Cette carte a jouer l'effet de la carte " ..carte_copie.Name.Value)
        return false    --pour pas carteblanche se fasse defaussé (A REVOIR !!)
    else
        Game.PlayCardVirtual(EffectOwner, carte_copie)
        This.Description.TryChangeValue(base_description)
    end
end

