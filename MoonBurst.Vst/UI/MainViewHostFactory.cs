﻿using MoonBurst.Api.Client;
using MoonBurst.ViewModel.Interface;

namespace MoonBurst.Vst
{
    public class VstMainViewHostFactory : IMainViewHostFactory
    {
        private readonly IMainViewModel mainViewModel;
        private readonly IFactory<IMainView> viewFactory;

        public VstMainViewHostFactory(IMainViewModel mainViewModel, IFactory<IMainView> viewFactory)
        {
            this.mainViewModel = mainViewModel;
            this.viewFactory = viewFactory;
        }
        public IMainViewHost Build()
        {
            return new UserControlMainViewHost(mainViewModel, viewFactory);
        }
    }
}