import PropTypes from 'prop-types';

function Header({ account, onLogin, onLogout, inProgress }) {
  return (
    <header className="top-bar">
      <div className="brand">
        <span className="brand-accent" />
        <h1>Pulse</h1>
      </div>
      <nav className="actions">
        {account ? (
          <>
            <span className="greeting">{account.name}</span>
            <button className="ghost" type="button" onClick={onLogout} disabled={inProgress !== 'none'}>
              Sign out
            </button>
          </>
        ) : (
          <button className="primary" type="button" onClick={onLogin} disabled={inProgress !== 'none'}>
            Sign in
          </button>
        )}
      </nav>
    </header>
  );
}

Header.propTypes = {
  account: PropTypes.object,
  onLogin: PropTypes.func.isRequired,
  onLogout: PropTypes.func.isRequired,
  inProgress: PropTypes.string
};

export default Header;
