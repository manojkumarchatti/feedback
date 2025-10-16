import { useState } from 'react';
import PropTypes from 'prop-types';

const initialState = {
  title: '',
  message: '',
  topicId: ''
};

function FeedbackForm({ topics, onSubmit, loading }) {
  const [form, setForm] = useState(initialState);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError('');
    setSuccess(false);

    if (!form.topicId) {
      setError('Select a topic to help us categorize your feedback.');
      return;
    }

    try {
      await onSubmit({ ...form, topicId: Number(form.topicId) });
      setForm(initialState);
      setSuccess(true);
    } catch (submitError) {
      setError(submitError.message ?? 'Unable to submit feedback.');
    }
  };

  return (
    <form className="form" onSubmit={handleSubmit}>
      <label>
        Title
        <input
          name="title"
          value={form.title}
          onChange={handleChange}
          required
          maxLength={200}
          placeholder="Summarize your idea"
        />
      </label>
      <label>
        Topic
        <select name="topicId" value={form.topicId} onChange={handleChange} required>
          <option value="" disabled>
            Choose a topic
          </option>
          {topics.map((topic) => (
            <option key={topic.id} value={topic.id}>
              {topic.name}
            </option>
          ))}
        </select>
      </label>
      <label>
        Message
        <textarea
          name="message"
          value={form.message}
          onChange={handleChange}
          required
          maxLength={4000}
          rows="6"
          placeholder="Share context, the impact, and any ideas to move forward"
        />
      </label>
      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">Thanks for your feedback!</div>}
      <button className="primary" type="submit" disabled={loading}>
        Submit feedback
      </button>
    </form>
  );
}

FeedbackForm.propTypes = {
  topics: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired
    })
  ),
  onSubmit: PropTypes.func.isRequired,
  loading: PropTypes.bool
};

FeedbackForm.defaultProps = {
  topics: [],
  loading: false
};

export default FeedbackForm;
