import { useMsal } from '@azure/msal-react';
import { useEffect, useMemo, useState } from 'react';
import { loginRequest } from './msalConfig.js';
import { AuthenticatedTemplate, UnauthenticatedTemplate } from '@azure/msal-react';
import Header from './components/Header.jsx';
import FeedbackTable from './components/FeedbackTable.jsx';
import FeedbackForm from './components/FeedbackForm.jsx';
import LoadingSpinner from './components/LoadingSpinner.jsx';
import { useFeedbackApi } from './hooks/useFeedbackApi.js';

function App() {
  const { instance, accounts, inProgress } = useMsal();
  const activeAccount = useMemo(() => accounts[0] ?? null, [accounts]);
  const [selectedId, setSelectedId] = useState(null);

  useEffect(() => {
    if (activeAccount) {
      instance.setActiveAccount(activeAccount);
    }
  }, [instance, activeAccount]);

  const { feedback, loading, topics, refresh, submitFeedback } = useFeedbackApi(activeAccount);

  const handleLogin = async () => {
    await instance.loginPopup(loginRequest);
  };

  const handleLogout = async () => {
    await instance.logoutPopup();
  };

  return (
    <div className="app-shell">
      <Header onLogin={handleLogin} onLogout={handleLogout} account={activeAccount} inProgress={inProgress} />
      <main className="content">
        <AuthenticatedTemplate>
          <section className="grid">
            <div className="card">
              <h2>Submit feedback</h2>
              <FeedbackForm topics={topics} onSubmit={submitFeedback} loading={loading} />
            </div>
            <div className="card">
              <h2>Team feedback</h2>
              {loading ? (
                <LoadingSpinner />
              ) : (
                <FeedbackTable
                  items={feedback}
                  onRefresh={refresh}
                  selectedId={selectedId}
                  onSelect={setSelectedId}
                />
              )}
            </div>
          </section>
        </AuthenticatedTemplate>
        <UnauthenticatedTemplate>
          <div className="hero">
            <h2>Sign in to share your perspective</h2>
            <p>Use your work account to submit feedback and collaborate with your team.</p>
            <button type="button" className="primary" onClick={handleLogin}>
              Sign in with Microsoft
            </button>
          </div>
        </UnauthenticatedTemplate>
      </main>
    </div>
  );
}

export default App;
