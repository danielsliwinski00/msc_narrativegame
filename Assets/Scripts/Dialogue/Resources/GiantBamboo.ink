EXTERNAL npcAction(name)
EXTERNAL getQuestStatus(questName)
EXTERNAL setQuestStatus(questName, questStatus)

VAR questStatus = ""

-> main

=== main ===
{questStatus == "complete":
~ setQuestStatus("questShaolin", "completeW")
[Screaming in agony]
AAAAAAAAAAAAAAAAAAAAAAAA!
OH HEAVENLY DEMON, WHY HAVE YOU FORSAKEN ME?!
~ npcAction("leave")
}
{questStatus == "fail":
~ setQuestStatus("questShaolin", "completeF")
THE DEMONIC SECT GROWS EVER STRONGER!
I'LL LET YOU LIVE... for now.
GET OUT OF HERE WEAKLING
~ npcAction("leave")
}
-> END