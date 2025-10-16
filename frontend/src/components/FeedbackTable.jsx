import PropTypes from 'prop-types';
import dayjs from 'dayjs';

function FeedbackTable({ items, onRefresh, onSelect, selectedId }) {
  return (
    <div className="table-wrapper">
      <div className="table-toolbar">
        <button className="ghost" type="button" onClick={onRefresh}>
          Refresh
        </button>
      </div>
      <table>
        <thead>
          <tr>
            <th>Title</th>
            <th>Topic</th>
            <th>Status</th>
            <th>Submitted</th>
            <th>By</th>
          </tr>
        </thead>
        <tbody>
          {items.map((item) => (
            <tr
              key={item.id}
              className={item.id === selectedId ? 'selected' : ''}
              onClick={() => onSelect(item.id)}
            >
              <td>{item.title}</td>
              <td>{item.topic}</td>
              <td>
                <span className={`status status-${item.status.toLowerCase()}`}>{item.status}</span>
              </td>
              <td>{dayjs(item.createdAt).format('MMM D, YYYY')}</td>
              <td>{item.submittedBy}</td>
            </tr>
          ))}
          {items.length === 0 && (
            <tr>
              <td colSpan="5" className="empty">
                No feedback yet. Encourage your team to share their thoughts.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

FeedbackTable.propTypes = {
  items: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      title: PropTypes.string.isRequired,
      topic: PropTypes.string.isRequired,
      status: PropTypes.string.isRequired,
      createdAt: PropTypes.string.isRequired,
      submittedBy: PropTypes.string.isRequired
    })
  ).isRequired,
  onRefresh: PropTypes.func.isRequired,
  onSelect: PropTypes.func.isRequired,
  selectedId: PropTypes.number
};

export default FeedbackTable;
