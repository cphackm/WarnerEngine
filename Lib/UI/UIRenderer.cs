﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using ProjectWarnerShared.Lib.Components;
using ProjectWarnerShared.Lib.Entities;
using ProjectWarnerShared.Scenes;
using ProjectWarnerShared.Services;

namespace ProjectWarnerShared.Lib.UI
{
    public class UIRenderer : ISceneEntity, IPreDraw, IDraw
    {
        public Func<IUIElement> Builder;
        private bool shouldDrawDeferred;

        private Dictionary<string, Dictionary<string, object>> componentEventStates;
        private Dictionary<string, Dictionary<string, object>> componentStates;

        private List<UIDrawCall> drawCalls;

        private bool wasUpdated;

        public UIRenderer(Func<IUIElement> Builder, bool ShouldDrawDeferred = false)
        {
            this.Builder = Builder;
            shouldDrawDeferred = ShouldDrawDeferred;
            componentEventStates = new Dictionary<string, Dictionary<string, object>>();
            componentStates = new Dictionary<string, Dictionary<string, object>>();
            drawCalls = new List<UIDrawCall>();
            wasUpdated = true;
        }

        public void OnAdd(Scene ParentScene) { }

        public void OnRemove(Scene ParentScene) { }

        public void PreDraw(float DT)
        {
            if (wasUpdated)
            {
                drawCalls = Builder().RenderAsRoot();

                // TODO: Clear states for components that were removed

                wasUpdated = false;
            }

            // Update the different elements in reverse draw order
            for (int i = drawCalls.Count - 1; i >= 0; i--)
            {
                UIDrawCall drawCall = drawCalls[i];
                drawCall.Element.PreDrawBase(DT, drawCall);
            }
        }

        public void Draw()
        {
            if (shouldDrawDeferred)
            {
                GameService.GetService<RenderService>().AddDeferredCall(_ => DrawImplementation());
            }
            else
            {
                DrawImplementation();
            }
        }

        public void DrawImplementation()
        {
            foreach (UIDrawCall drawCall in drawCalls)
            {
                drawCall.Draw();
            }
        }

        public BackingBox GetBackingBox()
        {
            return BackingBox.Dummy;
        }

        public bool IsVisible()
        {
            return true;
        }

        public void RegisterComponent(IUIElement Element)
        {
            if (!componentStates.ContainsKey(Element.GetKey()))
            {
                componentEventStates[Element.GetKey()] = new Dictionary<string, object>()
                {
                    { "isMouseInside", false },
                };
                componentStates[Element.GetKey()] = Element.GetDefaultState();
            }
        }

        public TState GetComponentEventState<TState>(string ComponentKey, string StateKey)
        {
            return (TState)componentEventStates[ComponentKey][StateKey];
        }

        public void SetComponentEventState<TState>(string ComponentKey, string StateKey, TState StateValue)
        {
            componentEventStates[ComponentKey][StateKey] = StateValue;
        }

        public TState GetComponentState<TState>(string ComponentKey, string StateKey)
        {
            return (TState)componentStates[ComponentKey][StateKey];
        }

        public void SetComponentState<TState>(string ComponentKey, string StateKey, TState StateValue)
        {
            TState oldState = (TState)componentStates[ComponentKey][StateKey];
            if ((oldState == null && StateValue != null) || (oldState != null && !oldState.Equals(StateValue)))
            {
                componentStates[ComponentKey][StateKey] = StateValue;
                wasUpdated = true;
            }
        }
    }
}
