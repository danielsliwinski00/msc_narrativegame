EXTERNAL setQuestStatus(questName, questStatus)
EXTERNAL getQuestStatus(questName)
EXTERNAL npcAction(name)

VAR questStatus = "incomplete"
-> main

=== main ===
~ setQuestStatus("questTang", "incomplete")
So you have arrived...
*[You were expecting me?]
[The Tang Clan member smiles knowingly.]
    Of course, our tang clan has many ways to acquire intel.
    You've come to see the patriarch haven't you?
    **[Yes I have a message from the Alliance]
The patriarch has given you a trial. You must prove that you can rely on more than just sight.
    Navigate the Tang clan labyrinth, which is shrouded in darkness. Use your other senses, sound and smell to guide you.
    You will encounter monsters along the way, defeat them to prove your strength.
    [The Tang Clan member leans slightly closer, emphasizing the weight of the task.]
    Only then will you be deemed worthy to meet the patriarch.
    ~ npcAction("leave")
->END