using Microsoft.Playwright;
using System.Threading.Tasks;

namespace yuki_qa_automation_tests.Pages
{
    public class HomePage : BasePage
    {
        // Selectors for common elements
        private const string WelcomeHeading = "h1, [data-testid='welcome-heading']";
        private const string SignInButton = "button:has-text('Sign In'), [data-testid='sign-in-button']";
        private const string SignUpButton = "button:has-text('Sign Up'), [data-testid='sign-up-button']";
        private const string NavMenu = "nav, [data-testid='navigation']";
        private const string SearchBox = "input[placeholder*='search'], [data-testid='search-input']";
        private const string FeaturedSection = "section[data-testid='featured'], .featured-section";

        public HomePage(IPage page, int defaultTimeout = 10000)
            : base(page, defaultTimeout)
        {
        }

        public async Task NavigateToAsync(string baseUrl)
        {
            await Page.GotoAsync(baseUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });
        }

        public async Task<bool> IsHomePageLoadedAsync()
        {
            // Check if key elements are visible
            return await IsElementVisibleAsync(WelcomeHeading);
        }

        public async Task<string> GetWelcomeHeadingAsync()
        {
            return await GetTextAsync(WelcomeHeading);
        }

        public async Task<bool> IsSignInButtonVisibleAsync()
        {
            return await IsElementVisibleAsync(SignInButton);
        }

        public async Task<bool> IsSignUpButtonVisibleAsync()
        {
            return await IsElementVisibleAsync(SignUpButton);
        }

        public async Task ClickSignInAsync()
        {
            await ClickAsync(SignInButton);
        }

        public async Task ClickSignUpAsync()
        {
            await ClickAsync(SignUpButton);
        }

        public async Task<bool> IsNavigationMenuVisibleAsync()
        {
            return await IsElementVisibleAsync(NavMenu);
        }

        public async Task<bool> IsSearchBoxPresentAsync()
        {
            return await IsElementPresentAsync(SearchBox);
        }

        public async Task SearchAsync(string query)
        {
            await FillAsync(SearchBox, query);
            await Page.Keyboard.PressAsync("Enter");
        }

        public async Task<bool> IsFeaturedSectionVisibleAsync()
        {
            return await IsElementVisibleAsync(FeaturedSection);
        }

        public async Task<string> GetPageHeadingAsync()
        {
            return await GetPageTitleAsync();
        }

        public async Task WaitForHomePageLoadAsync()
        {
            await WaitForLoadStateAsync(LoadState.NetworkIdle);
            await FindElementVisibleAsync(WelcomeHeading);
        }
    }
}
