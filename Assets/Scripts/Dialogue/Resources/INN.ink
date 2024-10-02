EXTERNAL npcAction(name)

-> main

=== main ===
Would you like to eat some Ramen? (Heal to full HP)
+ [Yes]
    ~ npcAction("healPlayer")
    -> END
+ [No I'm not hungry]
    -> END
