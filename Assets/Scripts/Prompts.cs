using System.Collections.Generic;
using UnityEngine;
using static Prompts;

public static class Prompts
{
    public enum Tone
    {
        Encouraging, 
        Indifferent, 
        Condescending, 
        Manipulative, 
        Doubtful
    }

    static Dictionary<Tone, string[]> tonePrompts = new Dictionary<Tone, string[]>()
    {
        { 
            Tone.Encouraging, 
            new string[] 
            {
                "You can do it.",
                "Good job. Keep going.",
                "You’ve done so well.",
                "This is fun, eh?",
                "Do your thing.",
                "Nice work. Keep going.",
                "Impressive.",
                "You are doing so well.",
            }
        },
        { 
            Tone.Indifferent, 
            new string[]
            {
                "You’ve come this far? Okay.",
                "Go ahead and pick up the pace.",
                "Don’t fall behind now.",
                "Wow nice. Now keep going.",
                "You’re doing alright. Keep at it.",
                "You will keep going.",
            }
        },         { 
            Tone.Condescending, 
            new string[]
            {
                "What’s the matter now? Keep going.",
                "You’re falling behind. Go faster.",
                "You used to be better.",
                "Do better like everyone else.",
                "Wow. I thought you’re better than this.",
                "For how long you’ve been doing this, I expected more from you.",
                "This is all you can do? Keep going.",
                "You want a treat or something? Keep going.",
                "How are you this bad? Look at everyone around you.",
            }
        },         { 
            Tone.Manipulative, 
            new string[]
            {
                "This is all you will do now.",
                "You can’t do anything else.",
                "Don’t even think about turning back.",
                "If you give up now, you’ll be a failure.",
                "You will keep going, just like everyone else.",
                "It’s too late to turn back.",
                "You won’t succeed at anything else.",
                "This is what everyone does. Who do you think you are to be different.",
                "Do you think you’re better than everyone else?",
                "Going back means failing.",
                "You’re better than giving up.",
                "Your life will be destroyed if you give up now.",
                "You don't have the courage to be different.", 
            } 
        },         { 
            Tone.Doubtful, 
            new string[]
            {
                "What are you doing?",
                "I can’t do this.",
                "What will you do now?",
                "I’m not made for this.",
                "Am I made for this?",
                "I’m not cut out for this.",
                "You’re not cut out for this.",
                "What do you think you’re doing?",
                "Don’t be special now.",
                "Do you think you’re special?", 
            } 
        }, 
    };

    public enum Condition
    {
        WrongDirection,
        InFog,
        InDeepFog,
        OmitTutorialCrateCoin,
        ProgressCanTransition,
    }

    static Dictionary<Condition, string[]> conditionPrompts = new Dictionary<Condition, string[]>()
    {
        {
            Condition.WrongDirection,
            new string[]
            {
                "<color=#9D2B29>Wrong way, buddy.</color>",
                "<color=#9D2B29>That's not forward.</color>", 
            }
        },
        {
            Condition.InFog,
            new string[] 
            {
                "<color=#9D2B29>Hurts, doesn't it?</color>",
                "<color=#9D2B29>Ouch, that must hurt.</color>",
                "<color=#9D2B29>Why make yourself suffer?</color>",
                "<color=#9D2B29>Don't do this to yourself.</color>",
            }
        }, 
        { 
            Condition.InDeepFog, 
            new string[]
            {
                "<color=#9D2B29>You need to get out now.</color>",
                "<color=#9D2B29>Back out while you still can.</color>",
                "<color=#9D2B29>Get out or you will hurt yourself.</color>", 
            }
        },        
        { 
            Condition.OmitTutorialCrateCoin, 
            new string[]
            {
                "Or don't. Fine.", 
                "Or don't. You're your own person I guess.", 
                "Or don't. I can't control you.", 
            }
        },        
        { 
            Condition.ProgressCanTransition, 
            new string[]
            {
                "<color=#9D2B29>Whatever you do, do not turn back.</color>",
                "<color=#9D2B29>Horrible things will happen if you go back.</color>",
                "<color=#9D2B29>Don't be different. Don't back out.</color>",
                "<color=#9D2B29>Who are you to think you can go back?</color>",
                "<color=#9D2B29>There is not a choice to go back.</color>",
                "<color=#9D2B29>Turn back? Pff. You wouldn't dare.</color>", 
            }
        },
    };

    public static string GetPrompt(Tone tone)
    {
        string[] prompts = tonePrompts[tone];
        return prompts[Random.Range(0, prompts.Length)];
    }

    public static string GetPrompt(Condition condition)
    {
        string[] prompts = conditionPrompts[condition];
        return prompts[Random.Range(0, prompts.Length)];
    }
}