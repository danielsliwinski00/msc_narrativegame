EXTERNAL npcAction(name)
EXTERNAL getQuestStatus(questName)
EXTERNAL setQuestStatus(questName, questStatus)

VAR questStatus = "complete"
-> main

=== main ===
~ getQuestStatus("questTang")
{questStatus == "success":
Don't test my patience... I will be on my way as soon as I can.
-> END
}
So you have arrived, Namgung Jin...
*[I believe you know the reason why]
Yes, I do.
    The demonic sect...
    We've been steadily building up our poison supplies. Our informers have already alerted us about their return.
    To see the Alliance leader actually call for us now... the situation must be dire.
    [With a determined expression, he straightens up.]
I will be on my way to the headquarters as soon as possible.
~ setQuestStatus("questTang", "success")
    -> END


