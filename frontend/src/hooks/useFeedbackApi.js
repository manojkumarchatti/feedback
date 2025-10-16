import { useCallback, useEffect, useState } from 'react';
import { useMsal } from '@azure/msal-react';
import { loginRequest } from '../msalConfig.js';
import { createClient } from '../api/client.js';

export function useFeedbackApi(account) {
  const { instance } = useMsal();
  const [feedback, setFeedback] = useState([]);
  const [topics, setTopics] = useState([]);
  const [loading, setLoading] = useState(false);

  const acquireToken = useCallback(async () => {
    if (!account) {
      return null;
    }
    const response = await instance.acquireTokenSilent({
      ...loginRequest,
      account
    });
    return response.accessToken;
  }, [instance, account]);

  const refresh = useCallback(async () => {
    if (!account) {
      return;
    }
    setLoading(true);
    try {
      const token = await acquireToken();
      const client = createClient(token);
      const [feedbackResponse, topicsResponse] = await Promise.all([
        client.get('/api/feedback'),
        client.get('/api/feedback/topics')
      ]);
      setFeedback(feedbackResponse.data);
      setTopics(topicsResponse.data);
    } catch (error) {
      console.error('Failed to load feedback', error);
    } finally {
      setLoading(false);
    }
  }, [account, acquireToken]);

  useEffect(() => {
    if (account) {
      refresh();
    } else {
      setFeedback([]);
      setTopics([]);
    }
  }, [account, refresh]);

  const submitFeedback = useCallback(
    async (payload) => {
      const token = await acquireToken();
      if (!token) {
        throw new Error('Sign in to submit feedback.');
      }
      const client = createClient(token);
      const response = await client.post('/api/feedback', payload);
      await refresh();
      return response.data;
    },
    [acquireToken, refresh]
  );

  return { feedback, topics, loading, refresh, submitFeedback };
}
